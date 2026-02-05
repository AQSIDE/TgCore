namespace TgCore.Api.Systems.Telemetry.Data;

public sealed class TelemetryUser
{
    public long Id { get; }
    public long TotalUpdates { get; internal set; }
    public long PeriodUpdates { get; internal set; }
    public string? Name { get; }
    public string? LanguageCode { get; }
    public bool IsPremium { get; }
    public TelemetryInteraction Interaction { get; }
    public DateTime FirstActive { get; }
    public DateTime LastActive { get; internal set; }

    internal TelemetryUser(long id, string? name, string? languageCode, bool isPremium, TelemetryConfig config)
    {
        Id = id;
        Name = name;
        LanguageCode = languageCode;
        IsPremium = isPremium;
        TotalUpdates = 1;
        Interaction = new TelemetryInteraction(config);

        FirstActive = DateTime.UtcNow;
    }
}

public sealed class TelemetryUserDto : ITelemetryEntity
{
    public long Id { get; init; }
    public string? Name  { get; init; }
    public long TotalUpdates { get; init; }
    public long PeriodUpdates { get; init; }
    public string? LanguageCode  { get; init; }
    public bool IsPremium { get; init; }
    public DateTime FirstActive { get; init; }
    public DateTime LastActive { get; init; }
    public TelemetryInteractionDto Interaction { get; init; }
}