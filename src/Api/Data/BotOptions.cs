using TgCore.Api.Systems.Telemetry;

namespace TgCore.Api.Data;

public sealed class BotOptions
{
    public ITelegramClient Client { get; }
    public IUpdateReceiver? UpdateReceiver { get; set; }
    public IBotLoopRunner? LoopRunner { get; set; }
    public ParseMode DefaultParseMode { get; set; }
    public IMessageLifetimeModule? Lifetime { get; set; }
    public IRateLimitModule? RateLimit { get; set; }
    public ITextFormatterModule? TextFormatter { get; set; }
    public ITemporaryMessageLimiterModule? TemporaryMessageLimiter { get; set; }
    
    public bool InitialUseTelemetry { get; set; }
    public TelemetryConfig? InitialTelemetryConfig { get; set; }

    public BotOptions(
        ITelegramClient client,
        IUpdateReceiver? updateReceiver = null,
        IBotLoopRunner? loopRunner = null,
        IMessageLifetimeModule? lifetime = null,
        IRateLimitModule? rateLimit = null,
        ITextFormatterModule? textFormatter = null,
        ITemporaryMessageLimiterModule? temporaryMessageLimiter = null)
    {
        Client = client;
        UpdateReceiver = updateReceiver ?? new LongPollingReceiver(Client, new[] { UpdateType.Message, UpdateType.CallbackQuery });
        LoopRunner = loopRunner ?? new BotLoopRunner();

        Lifetime = lifetime;
        RateLimit = rateLimit;
        TextFormatter = textFormatter;
        TemporaryMessageLimiter = temporaryMessageLimiter;
    }
}