namespace TgCore.Api.Data;

public class BotOptions
{
    public string Token { get; }
    public long Offset { get; set; } = 0;
    public int Timeout { get; set; } = 30;
    public UpdateType[] AllowedUpdates { get; set; }
    public ParseMode DefaultParseMode { get; set; }
    public ILifetimeModule? Lifetime { get; set; }
    public IRateLimitModule? RateLimit { get; set; }
    public ITemporaryMessageLimiterModule? TemporaryMessageLimiter { get; set; }

    public BotOptions(string token, UpdateType[]? allowedUpdates = null, ParseMode defaultParseMode = ParseMode.None,
        ILifetimeModule? lifetime = null,
        IRateLimitModule? rateLimit = null,
        ITemporaryMessageLimiterModule? temporaryMessageLimiter = null)
    {
        if (string.IsNullOrWhiteSpace(token))
            throw new ArgumentNullException(nameof(token));

        Token = token;
        AllowedUpdates = allowedUpdates ?? new[] { UpdateType.Message, UpdateType.CallbackQuery };
        DefaultParseMode = defaultParseMode;

        Lifetime = lifetime;
        RateLimit = rateLimit;
        TemporaryMessageLimiter = temporaryMessageLimiter;
    }
}