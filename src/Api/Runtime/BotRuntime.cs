namespace TgCore.Api.Runtime;

internal sealed class BotRuntime
{
    private readonly IUpdateReceiver _receiver;
    private readonly IBotLoopRunner _loopRunner;

    public BotRuntime(
        IUpdateReceiver receiver,
        IBotLoopRunner loopRunner)
    {
        _receiver = receiver;
        _loopRunner = loopRunner;
    }

    public async Task RunAsync(
        IReadOnlyList<Func<Update, Task>> updateHandlers,
        IReadOnlyList<Func<Exception, Task>> errorHandlers,
        IReadOnlyList<IBotLoop> loops,
        CancellationToken ct)
    {
        await Task.WhenAll(
            _receiver.StartReceiving(updateHandlers, errorHandlers, ct),
            _loopRunner.StartAsync(loops, errorHandlers, ct)
        );
    }
}