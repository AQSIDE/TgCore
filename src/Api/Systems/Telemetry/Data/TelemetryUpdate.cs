namespace TgCore.Api.Systems.Telemetry.Data;

public sealed class TelemetryUpdate
{
    public long Id { get; }
    public UpdateType Type { get; }
    public DateTime CreateDate { get; }

    internal TelemetryUpdate(long id, UpdateType type)
    {
        Id = id;
        Type = type;
        
        CreateDate = DateTime.Now;
    }
}

public sealed class TelemetryUpdateDto
{
    public long Id { get; init; }
    public UpdateType Type  { get; init; }
    public DateTime CreateDate  { get; init; }
}