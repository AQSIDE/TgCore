using System.Text;
using TgCore.Api.Bot;

namespace TestBot.Commands;

public class StartCommand : TelegramCommand
{
    public override string Name => "/start";

    public StartCommand(TelegramBot bot) : base(bot)
    {
    }

    public override async Task ExecuteAsync(long chatId, long? messageId, string[]? args = null)
    {
        StringBuilder sb = new StringBuilder();

        sb.AppendLine("👋 <b>Hello!</b> Available commands:");
        sb.AppendLine();
        sb.AppendLine("<b>/start</b> - Send this message");
        sb.AppendLine("<b>/get_me</b> - Send get me message");
        sb.AppendLine("<b>/text</b> - Send text message  ");
        sb.AppendLine("<b>/photo</b> - Send photo message");
        sb.AppendLine("<b>/video</b> - Send video message");
        sb.AppendLine("<b>/audio</b> - Send audio message");
        sb.AppendLine("<b>/document</b> - Send document message");
        sb.AppendLine("<b>/animation</b> - Send animation message");
        sb.AppendLine("<b>/media_group</b> - Send media group message");
        sb.AppendLine("<b>/poll</b> - Send poll message");
        sb.AppendLine("<b>/quiz</b> - Send quiz message");
        sb.AppendLine("<b>/inline</b> - Send inline markup message");
        sb.AppendLine("<b>/reply</b> - Send reply markup message");
        sb.AppendLine("<b>/dice</b> - Send dice message");

        await _bot.Requests.SendText(chatId, sb.ToString(), replyId: messageId);
    }
}