using TgCore.Api.Bot;
using TgCore.Api.Enums;

namespace TestBot.Commands;

public class DiceCommand : TelegramCommand
{
    public DiceCommand(TelegramBot bot) : base(bot)
    {
    }

    public override string Name => "/dice";
    public override async Task ExecuteAsync(long chatId, long? messageId, string[]? args = null)
    {
        var values = Enum.GetValues(typeof(DiceType));
        var random = new Random();
        var randomType = (DiceType)values.GetValue(random.Next(values.Length))!;
        
        await _bot.Requests.SendDice(chatId, randomType, messageId);
    }
}