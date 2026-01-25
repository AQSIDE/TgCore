namespace TgCore.Api.Services.Message;

public partial class MessageService
{
    public async Task<bool> AnswerCallback(string callbackId, string? text = null, bool showAlert = false)
    {
        try
        {
            await ApplyRateLimit();

            return await _bot.Client.CallAsync<bool>(TelegramMethods.ANSWER_CALLBACK_QUERY, new
            {
                callback_query_id = callbackId,
                text = text,
                show_alert = showAlert
            });
        }
        catch (Exception ex)
        {
            await _bot.AddException(ex, null);
            return false;
        }
    }
}