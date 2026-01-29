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

        sb.AppendLine("👋 _*Hello*_ Available commands:");
        sb.AppendLine();
        sb.AppendLine("*/start* - Send this message");
        sb.AppendLine("*/get\\_me* - Send get me message");
        sb.AppendLine("*/text* - Send text message");
        sb.AppendLine("*/photo* - Send photo message");
        sb.AppendLine("*/video* - Send video message");
        sb.AppendLine("*/audio* - Send audio message");
        sb.AppendLine("*/document* - Send document message");
        sb.AppendLine("*/animation* - Send animation message");
        sb.AppendLine("*/media\\_group* - Send media group message");
        sb.AppendLine("*/poll* - Send poll message");
        sb.AppendLine("*/quiz* - Send quiz message");
        sb.AppendLine("*/inline* - Send inline markup message");
        sb.AppendLine("*/reply* - Send reply markup message");

        await _bot.Requests.SendText(chatId, sb.ToString(), replyId: messageId);
    }
}