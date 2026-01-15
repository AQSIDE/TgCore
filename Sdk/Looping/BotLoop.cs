namespace TgCore.Sdk.Looping;

public abstract class BotLoop
{
    public int IntervalMs { get; set; }

    protected BotLoop(int intervalMs = 100)
    {
        IntervalMs = intervalMs;
    }

    internal async Task CallLoop()
    {
        await OnTick();
    }
    
    protected abstract Task OnTick();
}