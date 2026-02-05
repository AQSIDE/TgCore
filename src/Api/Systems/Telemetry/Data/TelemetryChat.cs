namespace TgCore.Api.Systems.Telemetry.Data;

public sealed class TelemetryChat
{
    public long Id { get; }
    public string? Title { get; }
    public ChatType Type { get; }
    public long TotalUpdates { get; internal set; }
    public long PeriodUpdates { get; internal set; }
    public TelemetryInteraction Interaction { get; }
    public DateTime FirstActive { get; }
    public DateTime LastActive { get; internal set; }

    public TelemetryChat(long id, string? title, ChatType type, TelemetryConfig config)
    {
        Id = id;
        Title = title;
        Type = type;
        TotalUpdates = 1;
        Interaction = new TelemetryInteraction(config);
        
        FirstActive = DateTime.UtcNow;
    }
}

public sealed class TelemetryChatDto : ITelemetryEntity
{
    public long Id { get; init; }
    public string? Name { get; init; }
    public ChatType Type { get; init; }
    public long TotalUpdates { get; init; }
    public long PeriodUpdates { get; init; }
    public TelemetryInteractionDto Interaction { get; init; }
    public DateTime FirstActive { get; init; }
    public DateTime LastActive { get; init; }
}