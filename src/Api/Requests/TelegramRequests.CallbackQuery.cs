namespace TgCore.Api.Requests;

public partial class TelegramRequests
{
    public async Task<RequestResponse<bool>> AnswerCallbackQuery(string callbackId, string? text = null, bool showAlert = false, CancellationToken ct = default)
    {
        return await SendRequest<bool>(TelegramMethods.ANSWER_CALLBACK_QUERY, new
        {
            callback_query_id = callbackId,
            text = text,
            show_alert = showAlert
        }, ct);
    }
}