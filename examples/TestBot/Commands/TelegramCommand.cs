using TgCore.Api.Bot;

namespace TestBot.Commands;

public abstract class TelegramCommand : ITelegramCommand
{
    protected readonly TelegramBot _bot;
    
    public abstract string Name { get; }
    
    protected TelegramCommand(TelegramBot bot)
    {
        _bot = bot;
    }
    
    public abstract Task ExecuteAsync(long chatId, long? messageId, string[]? args = null);
}