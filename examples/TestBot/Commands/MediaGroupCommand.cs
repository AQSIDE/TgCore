using TgCore.Api.Bot;
using TgCore.Api.Enums;
using TgCore.Api.Types.File;

namespace TestBot.Commands;

public class MediaGroupCommand : TelegramCommand
{
    public MediaGroupCommand(TelegramBot bot) : base(bot)
    {
    }

    public override string Name => "/media_group";
    public override async Task ExecuteAsync(long chatId, long? messageId, string[]? args = null)
    {
        await _bot.Requests.SendMediaGroup(chatId, new []
        {
            InputFile.FromUrl(InputFileType.Photo, "https://picsum.photos/600/400", true), 
            InputFile.FromUrl(InputFileType.Photo, "https://picsum.photos/800/600", true), 
            InputFile.FromUrl(InputFileType.Video, "https://sample-videos.com/video321/mp4/720/big_buck_bunny_720p_1mb.mp4", true), 
        }, caption: "This is media group");
    }
}