namespace TgCore.Api.Runtime;

public class BotLoopRunner : IBotLoopRunner
{
    public async Task StartAsync(
        IReadOnlyList<IBotLoop> loops,
        IReadOnlyList<Func<Exception, CancellationToken, Task>> errorHandlers,
        CancellationToken ct)
    {
        var tasks = loops.Select(loop =>
            Task.Run(async () =>
            {
                while (!ct.IsCancellationRequested)
                {
                    try
                    {
                        await loop.OnTick(ct);
                    }
                    catch (Exception ex)
                    {
                        await Task.WhenAll(errorHandlers.Select(h => h(ex, ct)));
                    }

                    await Task.Delay(loop.IntervalMs, ct);
                }
            }, ct)
        );

        await Task.WhenAll(tasks);
    }
}