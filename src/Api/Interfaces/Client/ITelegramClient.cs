using TgCore.Api.Systems.Telemetry;

namespace TgCore.Api.Interfaces.Client;

public interface ITelegramClient
{
    string ApiUrl { get; }
    string FileUrl { get; }
    Task<T> CallAsync<T>(
        string method, 
        TelemetrySystem telemetry,
        object? body = null, 
        JsonSerializerOptions? options = null, 
        CancellationToken ct = default);
}