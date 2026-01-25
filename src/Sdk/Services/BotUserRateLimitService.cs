namespace TgCore.Sdk.Services;

public class BotUserRateLimitService
{
    public (bool ok, TimeSpan timeLeft) Check(DateTime lastActivity, double seconds)
    {
        var elapsed = DateTime.Now - lastActivity;
    
        if (elapsed.TotalSeconds < seconds)
        {
            var waitSeconds = seconds - elapsed.TotalSeconds;
            return (false, TimeSpan.FromSeconds(waitSeconds));
        }
        
        return (true, TimeSpan.Zero);
    }
}