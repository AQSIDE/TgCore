using TgCore.Api.Bot;
using TgCore.Api.Keyboards.Inline;

namespace TestBot.Commands;

public class InlineCommand : TelegramCommand
{
    public InlineCommand(TelegramBot bot) : base(bot)
    {
    }

    public override string Name => "/inline";
    public override async Task ExecuteAsync(long chatId, long? messageId, string[]? args = null)
    {
        await _bot.Requests.SendText(chatId, "This inline markup", keyboard:
            InlineKeyboard.Create()
                .Row(InlineButton.CreateUrl("Link", "https://github.com/AQSIDE"))
                .Row(InlineButton.CreateData("Button 1", "btn1"), InlineButton.CreateData("Button 2", "btn1"))
                .Build(), replyId:messageId);
    }
}