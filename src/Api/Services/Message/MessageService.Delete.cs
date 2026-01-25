namespace TgCore.Api.Services.Message;

public partial class MessageService
{
    public async Task<bool> DeleteMessage(long chatId, long messageId)
    {
        try
        {
            await ApplyRateLimit();
            await UnregisterLifetime(chatId, messageId);

            return await _bot.Client.CallAsync<bool>(TelegramMethods.DELETE_MESSAGE, new
            {
                chat_id = chatId,
                message_id = messageId
            });
        }
        catch (Exception ex)
        {
            await _bot.AddException(ex, null);
            return false;
        }
    }
}