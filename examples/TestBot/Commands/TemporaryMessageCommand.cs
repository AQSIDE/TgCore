using TgCore.Api.Bot;

namespace TestBot.Commands;

public class TemporaryMessageCommand : TelegramCommand
{
    public TemporaryMessageCommand(TelegramBot bot) : base(bot)
    {
    }

    public override string Name => "/temporary_message";
    public override async Task ExecuteAsync(long chatId, long? messageId, string[]? args = null)
    {
        await _bot.Requests.SendText(chatId, "This message delete after 5s", lifeTime:TimeSpan.FromSeconds(5));
    }
}