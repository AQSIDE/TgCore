namespace TgCore.Api.Requests;

public partial class TelegramRequests
{
    public async Task<RequestResponse<bool>> DeleteMessage(long chatId, long messageId, CancellationToken ct = default)
    {
        await UnregisterLifetime(chatId, messageId);
        
        return await SendRequest<bool>(TelegramMethods.DELETE_MESSAGE, new
        {
            chat_id = chatId,
            message_id = messageId
        }, ct);
    }
    
    public async Task<RequestResponse<bool>> DeleteMessages(long chatId, long[] messageIds, CancellationToken ct = default)
    {
        foreach (var messageId in messageIds)
            await UnregisterLifetime(chatId, messageId);
        
        return await SendRequest<bool>(TelegramMethods.DELETE_MESSAGES, new
        {
            chat_id = chatId,
            message_ids = messageIds
        }, ct);
    }
}