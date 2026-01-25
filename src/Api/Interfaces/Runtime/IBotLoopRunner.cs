namespace TgCore.Api.Interfaces.Runtime;

public interface IBotLoopRunner
{
    Task StartAsync(
        IReadOnlyList<IBotLoop> loops,
        IReadOnlyList<Func<Exception, Task>> errorHandlers,
        CancellationToken ct);
}