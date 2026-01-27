using TgCore.Api.Requests.Parameters;

namespace TgCore.Api.Requests;

public partial class TelegramRequests
{
    public async Task<Message?> ForwardMessage(
        long chatId,
        long fromChatId,
        int messageId,
        DefaultParameters? defaultParameters)
    {
        try
        {
            await ApplyRateLimit();
            
            var parameters = new TelegramParametersBuilder()
                .Add("chat_id", chatId)
                .Add("from_chat_id", fromChatId)
                .Add("message_id", messageId)
                .AddDictionary(defaultParameters?.ToDictionary())
                .Build();

            var message = await _bot.Client.CallAsync<Message?>(TelegramMethods.FORWARD_MESSAGE, parameters);

            return message;
        }
        catch (Exception ex)
        {
            await _bot.AddException(ex);
            return null;
        }
    }
    
    public async Task<Message?> CopyMessage(
        long chatId,
        long fromChatId,
        int messageId,
        ParseMode? parseMode = null,
        IKeyboardMarkup? keyboard = null,
        DefaultParameters? defaultParameters = null)
    {
        try
        {
            await ApplyRateLimit();
            
            var parameters = new TelegramParametersBuilder()
                .Add("chat_id", chatId)
                .Add("from_chat_id", fromChatId)
                .Add("message_id", messageId)
                .Add("parse_mode", BotHelper.GetParseModeName(parseMode ?? _bot.Options.DefaultParseMode))
                .Add("reply_markup", keyboard)
                .AddDictionary(defaultParameters?.ToDictionary())
                .Build();

            var message = await _bot.Client.CallAsync<Message?>(TelegramMethods.COPY_MESSAGE, parameters);

            return message;
        }
        catch (Exception ex)
        {
            await _bot.AddException(ex);
            return null;
        }
    }
}