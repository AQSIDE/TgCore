namespace TgCore.Api.Data;

public sealed class BotOptions
{
    public ITelegramClient Client { get; }
    public IUpdateReceiver? UpdateReceiver { get; set; }
    public IBotLoopRunner? LoopRunner { get; set; }
    public ParseMode DefaultParseMode { get; set; }
    public ILifetimeModule? Lifetime { get; set; }
    public IRateLimitModule? RateLimit { get; set; }
    public ITemporaryMessageLimiterModule? TemporaryMessageLimiter { get; set; }

    public BotOptions(
        ITelegramClient client,
        IUpdateReceiver? updateReceiver = null,
        IBotLoopRunner? loopRunner = null,
        ILifetimeModule? lifetime = null,
        IRateLimitModule? rateLimit = null,
        ITemporaryMessageLimiterModule? temporaryMessageLimiter = null)
    {
        Client = client;
        UpdateReceiver = updateReceiver ?? new LongPollingReceiver(Client, new[] { UpdateType.Message, UpdateType.CallbackQuery });
        LoopRunner = loopRunner ?? new BotLoopRunner();

        Lifetime = lifetime;
        RateLimit = rateLimit;
        TemporaryMessageLimiter = temporaryMessageLimiter;
    }
}