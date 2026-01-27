using TgCore.Api.Bot;
using TgCore.Api.Data;
using TgCore.Api.Enums;
using TgCore.Api.Types.File;

namespace TestBot.Commands;

public class AnimationCommand : TelegramCommand
{
    public AnimationCommand(TelegramBot bot) : base(bot)
    {
    }

    public override string Name => "/animation";

    public override async Task ExecuteAsync(long chatId, long? messageId, string[]? args = null)
    {
        await _bot.Requests.SendMedia(chatId,
            InputFile.FromUrl(InputFileType.Animation, "https://media.giphy.com/media/ICOgUNjpvO0PC/giphy.gif"),
            caption: "🌀 GIF animation",
            replyId: messageId
        );
    }
}