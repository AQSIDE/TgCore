using TgCore.Api.Bot;

namespace TestBot.Commands;

public class ErrorCommand :  TelegramCommand
{
    public ErrorCommand(TelegramBot bot) : base(bot)
    {
    }

    public override string Name => "/error";
    
    public override async Task ExecuteAsync(long chatId, long? messageId, string[]? args = null)
    {
        var rand = new Random();
        var r = rand.Next(0, 2);

        if (r == 0)
            await _bot.Requests.EditText(chatId, 123, "test");
        else
            await _bot.Requests.DeleteMessage(chatId, 123);
    }
}