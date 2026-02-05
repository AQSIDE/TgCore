namespace TgCore.Api.Interfaces.Runtime;

public interface IBotLoopRunner
{
    Task StartAsync(
        IReadOnlyList<IBotLoop> loops,
        IReadOnlyList<Func<Exception, CancellationToken, Task>> errorHandlers,
        CancellationToken ct);
}