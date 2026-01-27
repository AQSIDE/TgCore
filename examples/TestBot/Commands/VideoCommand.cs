using TgCore.Api.Bot;
using TgCore.Api.Enums;
using TgCore.Api.Types.File;

namespace TestBot.Commands;

public class VideoCommand : TelegramCommand
{
    public VideoCommand(TelegramBot bot) : base(bot)
    {
    }

    public override string Name => "/video";
    public override async Task ExecuteAsync(long chatId, long? messageId, string[]? args = null)
    {
        await _bot.Requests.SendMedia(chatId, InputFile.FromUrl(InputFileType.Video, "https://sample-videos.com/video321/mp4/720/big_buck_bunny_720p_1mb.mp4"),
            caption: "🎥 Sample video",
            replyId: messageId
        );
    }
}