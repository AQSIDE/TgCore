using TgCore.Api.Systems.Telemetry.Data;
using TgCore.Diagnostics.Debugger;

namespace TgCore.Api.Systems.Telemetry;

public sealed class TelemetrySystem
{
    private readonly TelemetrySystemAutoLogMessageBuilder _builder = new();
    private readonly TelemetrySnapshot _snapshot;
    private readonly TelegramBot _bot;

    private readonly object _lock = new();
    
    public bool Enabled { get; set; }
    public Func<TelemetrySnapshotDto, TelemetrySnapshotDto?, Task>? OnReport { get; set; }
    
    
    public DateTime BotStartTime => _bot.StartTime;
    public string? LastAutoLogMessage { get; private set; }
    public TelemetrySnapshotDto? LastSnapshot { get; private set; }
    public TelemetryConfig Config { get; }

    internal TelemetrySystem(TelegramBot bot, TelemetryConfig? config = null)
    {
        _bot = bot;
        Config = config ?? new TelemetryConfig();
        _snapshot = new TelemetrySnapshot(Config);

        bot.MainLoop.AddRepeatingTask(Config.Interval, Report, DateTime.Now.Add(Config.Interval));
    }

    public TelemetrySnapshotDto GetSnapshot()
    {
        lock (_lock) return _snapshot.Get();
    }

    public void Update(Action<TelemetrySnapshot> update)
    {
        if (!Enabled) return;
        lock (_lock) update(_snapshot);
    }

    private async Task Report()
    {
        if (!Enabled) return;

        TelemetrySnapshotDto snapshotCopy;
        lock (_lock)
        {
            snapshotCopy = _snapshot.Get();
            _snapshot.Reset();
        }
        
        if (Config.UseAutoLog) 
            AutoLog(snapshotCopy);

        try
        {
            if (OnReport != null)
                await OnReport(snapshotCopy, LastSnapshot);
        }
        catch (Exception ex)
        {
            await _bot.AddException(new Exception($"Telemetry OnReport error: {ex}", ex));
        }

        lock (_lock)
        {
            LastSnapshot = snapshotCopy;
        }
    }

    private void AutoLog(TelemetrySnapshotDto snapshot)
    {
        var message = _builder.OnReport(snapshot, this);
        
        LastAutoLogMessage = message;
        Debug.Console.LogInfo(message, new LogOptions { UseFullDate = true });
    }
}