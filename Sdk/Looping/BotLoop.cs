using TgCore.Api.Interfaces;

namespace TgCore.Sdk.Looping;

public abstract class BotLoop : IBotLoop
{
    public int IntervalMs { get; set; }

    protected BotLoop(int intervalMs = 100)
    {
        IntervalMs = intervalMs;
    }
    
    public abstract Task OnTick();
}