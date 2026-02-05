using TgCore.Api.Requests.Parameters;

namespace TgCore.Api.Requests;

public partial class TelegramRequests
{
    public async Task<RequestResponse<Message>> ForwardMessage(
        long chatId,
        long fromChatId,
        int messageId,
        DefaultParameters? defaultParameters = null,
        CancellationToken ct = default)
    {
        var parameters = new TelegramParametersBuilder()
            .Add("chat_id", chatId)
            .Add("from_chat_id", fromChatId)
            .Add("message_id", messageId)
            .AddDictionary(defaultParameters?.ToDictionary())
            .Build();

        return await SendRequest<Message>(TelegramMethods.FORWARD_MESSAGE, parameters, ct);
    }

    public async Task<RequestResponse<Message>> CopyMessage(
        long chatId,
        long fromChatId,
        int messageId,
        ParseMode? parseMode = null,
        IKeyboardMarkup? keyboard = null,
        DefaultParameters? defaultParameters = null,
        CancellationToken ct = default)
    {
        var parameters = new TelegramParametersBuilder()
            .Add("chat_id", chatId)
            .Add("from_chat_id", fromChatId)
            .Add("message_id", messageId)
            .Add("parse_mode", BotHelper.GetParseModeName(parseMode ?? _bot.Options.DefaultParseMode))
            .Add("reply_markup", keyboard)
            .AddDictionary(defaultParameters?.ToDictionary())
            .Build();

        return await SendRequest<Message>(TelegramMethods.COPY_MESSAGE, parameters, ct);
    }
}