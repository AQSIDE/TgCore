using TgCore.Api.Bot;
using TgCore.Api.Enums;
using TgCore.Api.Types.File;

namespace TestBot.Commands;

public class AudioCommand : TelegramCommand
{
    public AudioCommand(TelegramBot bot) : base(bot)
    {
    }

    public override string Name => "/audio";
    public override async Task ExecuteAsync(long chatId, long? messageId, string[]? args = null)
    {
        await _bot.Requests.SendMedia(chatId, InputFile.FromUrl(InputFileType.Audio,  "https://sample-videos.com/audio/mp3/wave.mp3"),
            caption: "🎵 Sample audio",
            replyId: messageId
        );
    }
}