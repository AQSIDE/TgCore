namespace TgCore.Api.Systems.Telemetry.Data;

public sealed class TelemetryInteraction
{
    private Queue<string> _lastContext { get; } = new();
    public int MessagesSent { get; set; }
    public int CommandsUsed { get; set; }
    public int InlineQueries { get; set; }
    public int CallbackQueries { get; set; }
    public int ChatMembershipChanges { get; set; }
    public int ChosenInlineResults { get; set; }
    public int PollAnswers { get; set; }
    public int Payments { get; set; }

    public IReadOnlyList<string> LastContext => _lastContext.ToArray();
    
    private readonly TelemetryConfig _config;

    internal TelemetryInteraction(TelemetryConfig config)
    {
        _config = config;
    }

    public void AddMessage(string? message)
    {
        if (message == null) return;

        if (_lastContext.Count >= _config.MaxInteractionContext)
            _lastContext.Dequeue();

        _lastContext.Enqueue(message);
    }
}

public sealed class TelemetryInteractionDto
{
    public List<string> LastContext { get; init; }
    public int MessagesSent { get; init; }
    public int CommandsUsed { get; init; }
    public int InlineQueries { get; init; }
    public int CallbackQueries { get; init; }
    public int ChatMembershipChanges { get; init; }
    public int ChosenInlineResults { get; init; }
    public int PollAnswers { get; init; }
    public int Payments { get; init; }
}