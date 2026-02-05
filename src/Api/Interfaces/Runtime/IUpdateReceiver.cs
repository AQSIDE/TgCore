using TgCore.Api.Systems.Telemetry;

namespace TgCore.Api.Interfaces.Runtime;

public interface IUpdateReceiver
{
    Task StartReceiving(
        IReadOnlyList<Func<Update, CancellationToken, Task>> updateHandlers,
        IReadOnlyList<Func<Exception, CancellationToken, Task>> errorHandlers,
        TelemetrySystem telemetry,
        CancellationToken ct);
}