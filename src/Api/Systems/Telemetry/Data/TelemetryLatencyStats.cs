namespace TgCore.Api.Systems.Telemetry.Data;

public sealed class TelemetryLatencyStats
{
    public double Avg { get; init; }
    public double Min { get; init; }
    public double Max { get; init; }

    public TelemetryLatencyStats(double avg, double min, double max)
    {
        Avg = avg;
        Min = min;
        Max = max;
    }
    
    public bool IsEmpty() => Avg == 0 && Min == 0 && Max == 0;
}