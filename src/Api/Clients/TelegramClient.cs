namespace TgCore.Api.Clients;

public class TelegramClient : ITelegramClient
{
    private readonly HttpClient _http;

    public TelegramClient(string token)
    {
        _http = new HttpClient
        {
            BaseAddress = new Uri($"https://api.telegram.org/bot{token}/")
        };
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