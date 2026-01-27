namespace TgCore.Api.Requests;

public partial class TelegramRequests
{
    public async Task<bool> AnswerCallbackQuery(string callbackId, string? text = null, bool showAlert = false)
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
            await _bot.AddException(ex);
            return false;
        }
    }
}