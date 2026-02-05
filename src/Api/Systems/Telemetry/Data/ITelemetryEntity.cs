namespace TgCore.Api.Systems.Telemetry.Data;

public interface ITelemetryEntity
{
    long Id { get; }
    string Name { get; }
    long TotalUpdates { get; }
    long PeriodUpdates { get; }
    DateTime LastActive { get; }
    TelemetryInteractionDto Interaction { get; }
}