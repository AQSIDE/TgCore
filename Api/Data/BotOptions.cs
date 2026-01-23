namespace TgCore.Api.Data;

public class BotOptions
{
    public string Token { get; }
    public long Offset { get; set; }
    public int Timeout { get; set; } = 30;
    public UpdateType[] AllowedUpdates { get; set; }
    public ParseMode DefaultParseMode { get; set; }
    public LifetimeOptions? Lifetime { get; set; }
    public RateLimitOptions? RateLimit { get; set; }
    
    public IRateLimitModule? RlModule => RateLimit?.Module;
    public ILifetimeModule? LtModule => Lifetime?.Module;

    public BotOptions(string token, UpdateType[]? allowedUpdates = null, ParseMode defaultParseMode = ParseMode.None, 
        LifetimeOptions? lifetime = null, 
        RateLimitOptions? rateLimit = null)
    {
        Token = token;
        AllowedUpdates = allowedUpdates ?? new [] { UpdateType.Message, UpdateType.CallbackQuery };
        DefaultParseMode = defaultParseMode;
        
        Lifetime = lifetime;
        RateLimit = rateLimit;
    }
}