using TgCore.Api.Bot;
using TgCore.Api.Keyboards.Reply;

namespace TestBot.Commands;

public class ReplyCommand : TelegramCommand
{
    public ReplyCommand(TelegramBot bot) : base(bot)
    {
    }

    public override string Name => "/reply";
    public override async Task ExecuteAsync(long chatId, long? messageId, string[]? args = null)
    {
        await _bot.Requests.SendText(chatId, "This reply markup", keyboard:
            ReplyKeyboard.Create()
                .Row(ReplyButton.CreateText("1"))
                .Row(ReplyButton.CreateText("2"), ReplyButton.CreateText("3"))
                .Build(),replyId:messageId);
    }
}