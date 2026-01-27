using TgCore.Api.Bot;
using TgCore.Api.Methods;
using TgCore.Api.Requests.Parameters;

namespace TestBot.Commands;

public class TextCommand: TelegramCommand
{
    public TextCommand(TelegramBot bot) : base(bot)
    {
    }

    public override string Name => "/text";
    public override async Task ExecuteAsync(long chatId, long? messageId, string[]? args = null)
    {
        await _bot.Requests.SendText(chatId, "This is a text message", replyId: messageId, defaultParameters:
            new DefaultParameters
            {
                MessageEffectId = MessageEffectId.ThumbsUp.ToString()
            });
    }
}