using TgCore.Api.Requests.Parameters;

namespace TgCore.Api.Requests;

public partial class TelegramRequests
{
    public async Task<RequestResponse<Message>> SendDice(
        long chatId,
        DiceType type,
        long? replyId = null,
        IKeyboardMarkup? keyboard = null,
        DefaultParameters? defaultParameters = null, 
        CancellationToken ct = default)
    {
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

        return await SendRequest<Message>(TelegramMethods.SEND_DICE, parameters, ct);
    }
}