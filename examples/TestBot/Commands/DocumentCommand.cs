using TgCore.Api.Bot;
using TgCore.Api.Data;
using TgCore.Api.Enums;
using TgCore.Api.Types.File;

namespace TestBot.Commands;

public class DocumentCommand : TelegramCommand
{
    public DocumentCommand(TelegramBot bot) : base(bot)
    {
    }

    public override string Name => "/document";
    public override async Task ExecuteAsync(long chatId, long? messageId, string[]? args = null)
    {
        await _bot.Requests.SendMedia(chatId, InputFile.FromUrl(InputFileType.Document, "https://www.w3.org/WAI/ER/tests/xhtml/testfiles/resources/pdf/dummy.pdf"),
            caption: "This is document",
            replyId: messageId
        );
    }
}