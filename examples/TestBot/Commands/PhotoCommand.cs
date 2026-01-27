using TgCore.Api.Bot;
using TgCore.Api.Data;
using TgCore.Api.Enums;
using TgCore.Api.Types.File;

namespace TestBot.Commands;

public class PhotoCommand : TelegramCommand
{
    public PhotoCommand(TelegramBot bot) : base(bot)
    {
    }

    public override string Name => "/photo";
    public override async Task ExecuteAsync(long chatId, long? messageId, string[]? args = null)
    {
        await _bot.Requests.SendMedia(chatId, InputFile.FromUrl(InputFileType.Photo, "https://picsum.photos/600/400"),
            caption: "📸 Photo",
            replyId: messageId
        );
    }
}