namespace TgCore.Api.Requests;

public partial class TelegramRequests
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
            await _bot.AddException(ex);
            return false;
        }
    }
    
    public async Task<bool> DeleteMessages(long chatId, long[] messageIds)
    {
        try
        {
            await ApplyRateLimit();

            foreach (var id in messageIds)
            {
                await UnregisterLifetime(chatId, id);
            }

            return await _bot.Client.CallAsync<bool>(TelegramMethods.DELETE_MESSAGE, new
            {
                chat_id = chatId,
                message_ids = messageIds
            });
        }
        catch (Exception ex)
        {
            await _bot.AddException(ex);
            return false;
        }
    }
}