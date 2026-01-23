namespace TgCore.Api.Clients;

internal sealed class TelegramClient
{
    private readonly HttpClient _http;
    private readonly Uri _baseUrl;
    private readonly BotOptions _options;
    private readonly string _token;

    private long _offset;

    public TelegramClient(BotOptions options)
    {
        _token = options.Token;
        _options = options;
        _offset = options.Offset;
        _baseUrl = new Uri($"https://api.telegram.org/bot{options.Token}/");
        _http = new HttpClient
        {
            BaseAddress = _baseUrl
        };
    }

    public async Task StartReceiving(
        List<Func<Update, Task>> updateHandlers, 
        List<Func<Exception, Update?, Task>> errorHandlers, 
        List<IBotLoop> loops,
        CancellationToken ct)
    {
        var updateTask = Task.Run(async () =>
        {
            while (!ct.IsCancellationRequested)
            {
                try
                {
                    var updates = await CallAsync<Update[]>(TelegramMethods.GET_UPDATES, new
                    {
                        offset = _offset,
                        timeout = _options.Timeout,
                        allowed_updates = BotHelper.GetAllowedUpdatesNames(_options.AllowedUpdates)
                    });

                    foreach (var update in updates)
                    {
                        try
                        {
                            BotHelper.SetUpdateType(update);
                            await Task.WhenAll(updateHandlers.Select(f => f(update)));
                        }
                        catch (Exception ex)
                        {
                            await Task.WhenAll(errorHandlers.Select(f => f(ex, update)));
                        }
                        finally
                        {
                            _offset = update.Id + 1;
                        }
                    }
                }
                catch (Exception ex)
                {
                    await Task.WhenAll(errorHandlers.Select(f => f(ex, null)));
                    await Task.Delay(5000, ct);
                }
            }
        }, ct);
        
        var loopTasks = loops.Select(loop =>
            Task.Run(async () =>
            {
                while (!ct.IsCancellationRequested)
                {
                    try
                    {
                        await loop.OnTick();
                    }
                    catch (Exception ex)
                    {
                        await Task.WhenAll(errorHandlers.Select(f => f(ex, null)));
                    }

                    await Task.Delay(loop.IntervalMs, ct);
                }
            }, ct)
        ).ToList();
        
        await Task.WhenAll(loopTasks.Append(updateTask));
    }

    public async Task<T> CallAsync<T>(string method, object? body = null, JsonSerializerOptions? options = null)
    {
        options ??= new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };

        HttpResponseMessage response;

        if (body != null)
        {
            var json = JsonSerializer.Serialize(body, options);
            response = await _http.PostAsync(method, new StringContent(json, Encoding.UTF8, "application/json"));
        }
        else
        {
            response = await _http.GetAsync(method);
        }

        if (!response.IsSuccessStatusCode)
        {
            var errorBody = await response.Content.ReadAsStringAsync();
            throw new Exception($"Telegram HTTP error: {errorBody}");
        }

        var stream = await response.Content.ReadAsStreamAsync();
        var apiResponse = await JsonSerializer.DeserializeAsync<TelegramResponse<T>>(stream, options);

        if (apiResponse == null) throw new Exception("Failed to deserialize Telegram response");
        if (!apiResponse.Ok) throw new Exception(apiResponse.Description);

        return apiResponse.Result;
    }
}