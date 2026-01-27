using TgCore.Api.Requests.Parameters;

namespace TgCore.Api.Requests;

public partial class TelegramRequests
{
    public async Task<Message?> SendDice(
        long chatId,
        DiceType type,
        long? replyId = null,
        IKeyboardMarkup? keyboard = null,
        DefaultParameters? defaultParameters = null)
    {
        try
        {
            await ApplyRateLimit();
            
            string ToEmoji() => type switch
            {
                DiceType.Dice => "🎲",
                DiceType.Dart => "🎯",
                DiceType.Basketball => "🏀",
                DiceType.Football => "⚽",
                DiceType.SlotMachine => "🎰",
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
            
            var parameters = new TelegramParametersBuilder()
                .Add("chat_id", chatId)
                .Add("emoji", ToEmoji())
                .Add("reply_to_message_id", replyId)
                .Add("reply_markup", keyboard)
                .AddDictionary(defaultParameters?.ToDictionary())
                .Build();

            var message = await _bot.Client.CallAsync<Message?>(TelegramMethods.SEND_DICE, parameters);

            return message;
        }
        catch (Exception ex)
        {
            await _bot.AddException(ex);
            return null;
        }
    }
}