namespace TgCore.Api.Systems.Telemetry;

public sealed class TelemetryConfig
{
    public TimeSpan Interval { get; }
    public bool UseAutoLog { get; set; }
    public int MaxRequests { get; set; }
    public int MaxErrors { get; set; }
    public int MaxUsers { get; set; }
    public  int MaxChats { get; set; }
    public int MaxUpdates { get; set; }
    public int MaxUpdateHandlers { get; set; }
    public int MaxErrorHandlers { get; set; }
    public int MaxInteractionContext { get; set; }
    public bool AllowPrivateChat { get; set; }

    public TelemetryConfig(
        bool useAutoLog = true, 
        TimeSpan? interval = null, 
        int maxRequests = 30, 
        int maxUpdates = 30, 
        int maxUpdateHandlers = 30,
        int maxErrorHandlers = 30,
        int maxUsers = 30, 
        int maxChats = 30,
        int maxErrors = 30,
        int maxInteractionContext = 10,
        bool allowPrivateChat = false)
    {
        UseAutoLog = useAutoLog;
        Interval = interval ?? TimeSpan.FromMinutes(5);
        MaxRequests = maxRequests;
        MaxUsers = maxUsers;
        MaxChats = maxChats;
        MaxUpdates = maxUpdates;
        MaxUpdateHandlers = maxUpdateHandlers;
        MaxErrorHandlers = maxErrorHandlers;
        MaxErrors = maxErrors;
        MaxInteractionContext = maxInteractionContext;
        AllowPrivateChat = allowPrivateChat;
    }
}