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

    public TelegramBot Build()
    {
        var bot = new TelegramBot(_options);

        return bot;
    }
}