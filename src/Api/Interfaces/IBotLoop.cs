namespace TgCore.Api.Interfaces;

public interface IBotLoop
{
    int IntervalMs { get; }
    Task OnTick();
}