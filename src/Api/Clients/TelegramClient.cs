using System.Diagnostics;
using TgCore.Api.Exceptions;
using TgCore.Api.Systems.Telemetry;
using TgCore.Api.Systems.Telemetry.Data;

namespace TgCore.Api.Clients;

public class TelegramClient : ITelegramClient
{
    private readonly JsonSerializerOptions _defaultOptions;
    private readonly HttpClient _http;

    public string ApiUrl { get; }
    public string FileUrl { get; }

    public TelegramClient(string token,
        string apiBaseUrl = "https://api.telegram.org",
        string fileBaseUrl = "https://api.telegram.org/file",
        JsonSerializerOptions? defaultOptions = null)
    {
        if (string.IsNullOrWhiteSpace(token))
            throw new ArgumentException("Token cannot be empty", nameof(token));

        ApiUrl = $"{apiBaseUrl.TrimEnd('/')}/bot{token}/";
        FileUrl = $"{fileBaseUrl.TrimEnd('/')}/bot{token}/";

        _defaultOptions = defaultOptions ?? new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };

        _http = new HttpClient();
    }

    public async Task<T> CallAsync<T>(
        string method,
        TelemetrySystem telemetry,
        object? body = null,
        JsonSerializerOptions? options = null,
        CancellationToken ct = default)
    {
        var sw = Stopwatch.StartNew();
        options ??= _defaultOptions;

        var isSuccess = false;
        var errorMessage = string.Empty;

        try
        {
            HttpResponseMessage response;
            var url = $"{ApiUrl}{method}";

            if (body != null)
            {
                var json = JsonSerializer.Serialize(body, options);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                
                response = await _http.PostAsync(url, content, ct);
            }
            else
            {
                response = await _http.GetAsync(url, ct);
            }
            
            var raw = await response.Content.ReadAsStringAsync(ct);
            
            var apiResponse = JsonSerializer.Deserialize<TelegramResponse<T>>(raw, options);
            
            if (apiResponse == null)
                throw new InvalidOperationException("Failed to deserialize Telegram response");

            if (!apiResponse.Ok)
            {
                var ex =  new TelegramApiException("Telegram API error", apiResponse.ErrorCode, apiResponse.Description, method);
                
                telemetry.Update(s => s.AddError(new TelemetryError(ex, ex)));
                throw ex;
            }
            
            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Telegram HTTP Error ({response.StatusCode}, {method}): {raw}");
            }

            isSuccess = true;
            return apiResponse.Result!;
        }
        catch (Exception ex)
        {
            errorMessage = ex.Message;
            throw;
        }
        finally
        {
            sw.Stop();

            if (method != TelegramMethods.GET_UPDATES)
            {
                telemetry.Update(s => s.AddRequest(new TelemetryRequest(
                    method,
                    sw.ElapsedMilliseconds,
                    isSuccess,
                    errorMessage
                )));
            }
        }
    }
}