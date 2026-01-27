using TgCore.Api.Modules;

namespace TgCore.Api.Bot;

public class TelegramBotBuilder
{
    private readonly BotOptions _options;

    public TelegramBotBuilder(ITelegramClient client)
    {
        _options = new BotOptions(client);
    }

    public TelegramBotBuilder UseDefaultParseMode(ParseMode mode)
    {
        _options.DefaultParseMode = mode;
        return this;
    }

    public TelegramBotBuilder UseUpdateReceiver(IUpdateReceiver receiver)
    {
        _options.UpdateReceiver = receiver;
        return this;
    }

    public TelegramBotBuilder UseLoopRunner(IBotLoopRunner runner)
    {
        _options.LoopRunner = runner;
        return this;
    }

    public TelegramBotBuilder UseRateLimit(IRateLimitModule? module = null)
    {
        _options.RateLimit = module ?? new RateLimitModule();
        return this;
    }

    public TelegramBotBuilder UseLifetime(ILifetimeModule? module = null)
    {
        _options.Lifetime = module;
        return this;
    }

    public TelegramBotBuilder UseTemporaryMessageLimiter(ITemporaryMessageLimiterModule? module = null)
    {
        _options.TemporaryMessageLimiter = module ?? new TemporaryMessageLimiterModule();
        return this;
    }

    public TelegramBot Build()
    {
        var bot = new TelegramBot(_options);
        
        if (_options.Lifetime == null)
            bot.Options.Lifetime = new LifetimeModule(bot, bot.MainLoop);

        return bot;
    }
}