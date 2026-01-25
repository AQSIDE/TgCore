namespace TgCore.Api.Data;

public sealed class BotOptions
{
    public string Token { get; }
    public ITelegramClient Client { get; }
    public IUpdateReceiver UpdateReceiver { get; }
    public IBotLoopRunner LoopRunner { get; }
    public UpdateType[] AllowedUpdates { get; set; }
    public ParseMode DefaultParseMode { get; set; }
    public ILifetimeModule? Lifetime { get; set; }
    public IRateLimitModule? RateLimit { get; set; }
    public ITemporaryMessageLimiterModule? TemporaryMessageLimiter { get; set; }

    public BotOptions(string token,
        ITelegramClient? client = null,
        IUpdateReceiver? updateReceiver = null,
        IBotLoopRunner? loopRunner = null,
        UpdateType[]? allowedUpdates = null,
        ParseMode defaultParseMode = ParseMode.None,
        ILifetimeModule? lifetime = null,
        IRateLimitModule? rateLimit = null,
        ITemporaryMessageLimiterModule? temporaryMessageLimiter = null)
    {
        if (string.IsNullOrWhiteSpace(token))
            throw new ArgumentNullException(nameof(token));

        Token = token;

        AllowedUpdates = allowedUpdates ?? new[] { UpdateType.Message, UpdateType.CallbackQuery };
        DefaultParseMode = defaultParseMode;

        Client = client ?? new TelegramClient(token);
        UpdateReceiver = updateReceiver ?? new LongPollingReceiver(Client, AllowedUpdates, 0, 30);
        LoopRunner = loopRunner ?? new BotLoopRunner();

        Lifetime = lifetime;
        RateLimit = rateLimit;
        TemporaryMessageLimiter = temporaryMessageLimiter;
    }
}