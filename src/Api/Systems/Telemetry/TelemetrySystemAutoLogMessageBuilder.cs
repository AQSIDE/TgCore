using TgCore.Api.Systems.Telemetry.Data;

namespace TgCore.Api.Systems.Telemetry;

internal class TelemetrySystemAutoLogMessageBuilder
{
    public string OnReport(TelemetrySnapshotDto snapshot, TelemetrySystem telemetry)
    {
        var sb = new StringBuilder();

        // --- Prepare stats ---
        var typeStats = snapshot.Updates
            .GroupBy(u => u.Type)
            .Select(g => new { Type = g.Key, Count = g.Count() })
            .OrderByDescending(x => x.Count)
            .Take(10)
            .ToList();

        var apiErrors = snapshot.ApiErrors;
        var localErrors = snapshot.LocalErrors;

        var activeUsers = snapshot.Users
            .OrderByDescending(u => u.TotalUpdates)
            .ThenByDescending(u => u.LastActive)
            .Take(5)
            .ToList();
        
        var lastUsers = snapshot.Users
            .OrderByDescending(u => u.LastActive)
            .Take(10)
            .ToList();
        
        var activeChats = snapshot.Chats
            .OrderByDescending(u => u.TotalUpdates)
            .ThenByDescending(u => u.LastActive)
            .Take(5)
            .ToList();
        
        var lastChats = snapshot.Chats
            .OrderByDescending(u => u.LastActive)
            .Take(10)
            .ToList();

        var requestStats = snapshot.Requests
            .GroupBy(r => r.Method)
            .Select(g => new { Method = g.Key, Count = g.Count() })
            .OrderByDescending(x => x.Count)
            .Take(10)
            .ToList();

        var deltaUpdates = telemetry.LastSnapshot != null ? snapshot.PeriodUpdates - telemetry.LastSnapshot.PeriodUpdates : 0;
        var deltaRequest = telemetry.LastSnapshot != null ? snapshot.PeriodRequests - telemetry.LastSnapshot.PeriodRequests : 0;
        var deltaUsers   = telemetry.LastSnapshot != null ? snapshot.PeriodUniqueUsers - telemetry.LastSnapshot.PeriodUniqueUsers : 0;
        var deltaChats   = telemetry.LastSnapshot != null ? snapshot.PeriodUniqueChats - telemetry.LastSnapshot.PeriodUniqueChats : 0;
        var deltaErrors  = telemetry.LastSnapshot != null ? snapshot.PeriodErrors - telemetry.LastSnapshot.PeriodErrors : 0;
        var deltaUpdPerUser = telemetry.LastSnapshot != null ? snapshot.UpdatesPerUser -telemetry.LastSnapshot.UpdatesPerUser : 0;
        var deltaReqPerUser = telemetry.LastSnapshot != null ? snapshot.RequestsPerUser - telemetry.LastSnapshot.RequestsPerUser : 0;
        var deltaUpdPerChat = telemetry.LastSnapshot != null ? snapshot.UpdatesPerChat -telemetry.LastSnapshot.UpdatesPerChat : 0;
        var deltaReqPerChat = telemetry.LastSnapshot != null ? snapshot.RequestsPerChat - telemetry.LastSnapshot.RequestsPerChat : 0;
        var deltaAvgUpdLatency =  telemetry.LastSnapshot != null ? snapshot.UpdateHandlerLatency.Avg - telemetry.LastSnapshot.UpdateHandlerLatency.Avg : 0;
        var deltaAvgErrLatency =  telemetry.LastSnapshot != null ? snapshot.ErrorHandlerLatency.Avg - telemetry.LastSnapshot.ErrorHandlerLatency.Avg : 0;
        var deltaAvgHttpLatency =  telemetry.LastSnapshot != null ? snapshot.HTTPLatency.Avg - telemetry.LastSnapshot.HTTPLatency.Avg : 0;

        // --- HEADER ---
        sb.AppendLine($"\n==============================");
        sb.AppendLine($"   TELEMETRY SNAPSHOT #{snapshot.Id}");
        sb.AppendLine($"==============================");

        // --- INFORMATION ---
        var uptime = DateTime.UtcNow - telemetry.BotStartTime;

        sb.AppendLine("INFORMATION (utc)");
        sb.AppendLine($"Bot start time: {telemetry.BotStartTime}");
        sb.AppendLine($"Timestamp: {snapshot.Timestamp}");
        sb.AppendLine($"Uptime: {FormatTimeSpan(uptime)}");
        sb.AppendLine($"Period: {FormatTimeSpan(telemetry.Config.Interval)}");
        sb.AppendLine(new string('-', 40));
        
        // --- LATENCY ---
        sb.AppendLine("LATENCY (ms)");
        if (!snapshot.HTTPLatency.IsEmpty())
        {
            sb.AppendLine($"   HTTP -> Avg: {snapshot.HTTPLatency.Avg:F2} ({GetDelta(deltaAvgHttpLatency)}) | Min: {snapshot.HTTPLatency.Min} | Max: {snapshot.HTTPLatency.Max}");
        }
        
        if (!snapshot.UpdateHandlerLatency.IsEmpty())
        {
            sb.AppendLine($"   Update Handlers -> Avg: {snapshot.UpdateHandlerLatency.Avg:F2} ({GetDelta(deltaAvgUpdLatency)}) | Min: {snapshot.UpdateHandlerLatency.Min} | Max: {snapshot.UpdateHandlerLatency.Max}");
        }
        
        if (!snapshot.ErrorHandlerLatency.IsEmpty())
        {
            sb.AppendLine($"   Error Handlers -> Avg: {snapshot.ErrorHandlerLatency.Avg:F2} ({GetDelta(deltaAvgErrLatency)}) | Min: {snapshot.ErrorHandlerLatency.Min} | Max: {snapshot.ErrorHandlerLatency.Max}");
        }
        
        sb.AppendLine(new string('-', 40));

        // --- UPDATES ---
        sb.AppendLine($"UPDATES (per. delta: {GetDelta(deltaUpdates)})");
        sb.AppendLine($"Period: {snapshot.PeriodUpdates} | Total: {snapshot.TotalUpdates}");
        sb.AppendLine($"UPS: period={CalculateUPS(snapshot.PeriodUpdates, telemetry.Config.Interval.TotalSeconds):F2}/s, avg={CalculateUPS(snapshot.TotalUpdates, uptime.TotalSeconds):F2}/s");    
        if (typeStats.Any())
        {
            sb.AppendLine("-> Top types:");
            foreach (var stat in typeStats)
            {
                sb.AppendLine($"   * {stat.Type} : {stat.Count}");
            }
        }

        sb.AppendLine(new string('-', 40));

        // --- UNIQUE USERS ---
        sb.AppendLine($"UNIQUE USERS (per. delta: {GetDelta(deltaUsers)})");
        sb.AppendLine($"Period: {snapshot.PeriodUniqueUsers} | Total: {snapshot.TotalUniqueUsers}");
        sb.AppendLine($"UPS: period={CalculateUPS(snapshot.PeriodUniqueUsers, telemetry.Config.Interval.TotalSeconds):F2}/s, avg={CalculateUPS(snapshot.TotalUniqueUsers, uptime.TotalSeconds):F2}/s");   
        sb.AppendLine($"UPD/USER: {snapshot.UpdatesPerUser:F2} ({GetDelta(deltaUpdPerUser)}), REQ/USER: {snapshot.RequestsPerUser:F2} ({GetDelta(deltaReqPerUser)})");  
        
        if (activeUsers.Any())
        {
            sb.AppendLine("-> Top users:");
            foreach (var user in activeUsers)
            {
                sb.AppendLine(FormatEntityInfo(user, telemetry, uptime, user.LanguageCode!));
            }
        }
        
        if (lastUsers.Any())
        {
            sb.AppendLine("-> Last users:");
            foreach (var user in lastUsers)
            {
                sb.AppendLine(FormatEntityInfo(user, telemetry, uptime, user.LanguageCode!, true, true));
            }
        }

        sb.AppendLine(new string('-', 40));
        
        // --- UNIQUE CHATS ---
        sb.AppendLine($"UNIQUE CHATS (per. delta: {GetDelta(deltaChats)})");
        sb.AppendLine($"Period: {snapshot.PeriodUniqueChats} | Total: {snapshot.TotalUniqueChats}");
        sb.AppendLine($"CPS: period={CalculateUPS(snapshot.PeriodUniqueChats, telemetry.Config.Interval.TotalSeconds):F2}/s, avg={CalculateUPS(snapshot.TotalUniqueChats, uptime.TotalSeconds):F2}/s");   
        sb.AppendLine($"UPD/CHAT: {snapshot.UpdatesPerChat:F2} ({GetDelta(deltaUpdPerChat)}), REQ/CHAT: {snapshot.RequestsPerChat:F2} ({GetDelta(deltaReqPerChat)})");  
        
        if (activeChats.Any())
        {
            sb.AppendLine("-> Top chats:");
            foreach (var chat in activeChats)
            {
                sb.AppendLine(FormatEntityInfo(chat, telemetry, uptime, chat.Type.ToString()));
            }
        }
        
        if (lastChats.Any())
        {
            sb.AppendLine("-> Last chats:");
            foreach (var chat in lastChats)
            {
                sb.AppendLine(FormatEntityInfo(chat, telemetry, uptime, chat.Type.ToString(), true, true));
            }
        }

        sb.AppendLine(new string('-', 40));

        // --- REQUESTS ---
        sb.AppendLine($"REQUESTS (per. delta: {GetDelta(deltaRequest)})");
        sb.AppendLine($"Period: {snapshot.PeriodRequests} | Success: {snapshot.TotalSuccessfulRequests} | Failed: {snapshot.TotalFailedRequests} | Total: {snapshot.TotalRequests}");
        sb.AppendLine($"RPS: period={CalculateUPS(snapshot.PeriodRequests, telemetry.Config.Interval.TotalSeconds):F2}/s, avg={CalculateUPS(snapshot.TotalRequests, uptime.TotalSeconds):F2}/s"); 
        
        if (requestStats.Any())
        {
            sb.AppendLine("-> Top methods:");
            foreach (var stat in requestStats)
            {
                var request = GetRequestStatsByMethod(snapshot, stat.Method);
                var total = GetTotalRequestCountByMethod(snapshot, stat.Method);
                var avgLatency = GetAverageLatencyByMethod(snapshot, stat.Method);

                var deltaAvgLatency = telemetry.LastSnapshot != null
                    ? avgLatency - GetAverageLatencyByMethod(telemetry.LastSnapshot, stat.Method)
                    : 0;

                var deltaCount = telemetry.LastSnapshot != null
                    ? total - GetTotalRequestCountByMethod(telemetry.LastSnapshot, stat.Method)
                    : 0;
                
                sb.AppendLine(
                    $"   * {stat.Method,-12} : {total} ({GetDelta(deltaCount)}) | " +
                    $"Success: {request.Success}, Fail: {request.Fail} | " +
                    $"Avg latency: {avgLatency:F2}ms ({GetDelta(deltaAvgLatency)})"
                );
            }
        }

        sb.AppendLine(new string('-', 40));

        // --- ERRORS ---
        sb.AppendLine($"ERRORS (per. delta: {GetDelta(deltaErrors)})");
        sb.AppendLine($"Period: {snapshot.PeriodErrors} | Total: {snapshot.TotalErrors}");
        if (apiErrors.Any())
        {
            sb.AppendLine("-> API errors:");
            foreach (var error in apiErrors)
            {
                sb.AppendLine($"   * [{error.ErrorCode}] {error.Method} : {error.Count}");
                sb.AppendLine($"      {error.Description}");
            }
        }
        
        if (localErrors.Any())
        {
            sb.AppendLine("-> Local errors:");

            foreach (var error in localErrors)
            {
                sb.AppendLine($"   * [{error.ExceptionType}] {error.Method ?? "unknown"} : {error.Count}");
                sb.AppendLine($"      {error.Message}");
            }
        }
        
        sb.AppendLine(new string('-', 40));
        
        return sb.ToString().TrimEnd();
    }

    private string FormatEntityInfo(ITelemetryEntity entity, TelemetrySystem telemetry, TimeSpan uptime, string extra, bool useShort = false, bool useContext = false)
    {
        StringBuilder sb = new StringBuilder();
        var timeSinceLastActive = DateTime.UtcNow - entity.LastActive;
        
        sb.AppendLine($"   * {entity.Name} ({entity.Id}) ({extra}) | Updates: period={entity.PeriodUpdates}, total={entity.TotalUpdates}");
        sb.AppendLine($"     UPS: period={CalculateUPS(entity.PeriodUpdates, telemetry.Config.Interval.TotalSeconds):F2}/s, avg={CalculateUPS(entity.TotalUpdates, uptime.TotalSeconds):F2}/s");
        sb.AppendLine($"     Last active: {FormatTimeSpan(timeSinceLastActive)} ago");
        
        if (!useShort)
        {
            AppendInteractionStats(sb, entity.Interaction);
        }

        if (useContext)
        {
            var ctx = FormatContext(entity.Interaction.LastContext);
            if (!string.IsNullOrEmpty(ctx))
                sb.AppendLine(ctx);
        }
        
        return sb.ToString().TrimEnd();
    }
    
    private void AppendInteractionStats(StringBuilder sb, TelemetryInteractionDto interaction)
    {
        if (interaction == null) return;
    
        var hasMessages = interaction.MessagesSent > 0;
        var hasCommands = interaction.CommandsUsed > 0;
        var hasCallbacks = interaction.CallbackQueries > 0;
        var hasInline = interaction.InlineQueries > 0 || interaction.ChosenInlineResults > 0;
        var hasPayments = interaction.Payments > 0;
        var hasPolls = interaction.PollAnswers > 0;
        
        var statsParts = new List<string>();
    
        if (hasMessages) statsParts.Add($"Messages: {interaction.MessagesSent}");
        if (hasCommands) statsParts.Add($"Commands: {interaction.CommandsUsed}");
        if (hasCallbacks) statsParts.Add($"Callback: {interaction.CallbackQueries}");
        if (hasInline) statsParts.Add($"Inline: {interaction.InlineQueries + interaction.ChosenInlineResults}");
        if (hasPayments) statsParts.Add($"Payments: {interaction.Payments}");
        if (hasPolls) statsParts.Add($"Poll: {interaction.PollAnswers}");
    
        if (statsParts.Any())
        {
            sb.AppendLine($"      {string.Join(", ", statsParts)}");
        }
    }

    private double CalculateUPS(long total, double seconds)
    {
        return seconds > 0 ? total / seconds : 0;
    }

    private string GetDelta(double number, int decimals = 0)
    {
        var format = decimals > 0 ? $"F{decimals}" : "0";
        var value = number.ToString(format);
    
        return number switch
        {
            > 0 => $"+{value}", 
            < 0 => $"{value}", 
            _ => "+0"      
        };
    }
    
    private double GetAverageLatencyByMethod(TelemetrySnapshotDto snapshot, string method)
    {
        var methodRequests = snapshot.Requests.Where(r => r.Method == method).ToList();
        return methodRequests.Any() ? methodRequests.Average(r => r.LatencyMs) : 0;
    }

    private (int Success, int Fail) GetRequestStatsByMethod(TelemetrySnapshotDto snapshot, string method)
    {
        var success = snapshot.Requests.Count(r => r.Method == method && r.IsSuccess);
        var fail = snapshot.Requests.Count(r => r.Method == method && !r.IsSuccess);
        return (success, fail);
    }
    
    private int GetTotalRequestCountByMethod(TelemetrySnapshotDto snapshot, string method)
    {
        return snapshot.Requests.Count(r => r.Method == method);
    }
    
    private string FormatContext(IEnumerable<string>? context, int maxMessageLength = 30, int maxMessages = 5)
    {
        if (context == null || !context.Any())
            return string.Empty;

        string Truncate(string s) => s.Length <= maxMessageLength ? s : s.Substring(0, maxMessageLength) + "...";

        var last = context
            .TakeLast(maxMessages)
            .Select(m => $"\"{Truncate(m)}\"");

        return $"      CTX: {string.Join(", ", last)}";
    }
    
    private string FormatTimeSpan(TimeSpan span)
    {
        if (span.TotalDays >= 1)
            return $"{(int)span.TotalDays}d {span.Hours}h";
        if (span.TotalHours >= 1)
            return $"{(int)span.TotalHours}h {span.Minutes}m";
        if (span.TotalMinutes >= 1)
            return $"{(int)span.TotalMinutes}m {span.Seconds}s";
    
        return $"{(int)span.TotalSeconds}s";
    }
}