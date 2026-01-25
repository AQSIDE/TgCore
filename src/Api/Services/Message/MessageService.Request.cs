namespace TgCore.Api.Services.Message;

public partial class MessageService
{
    public async Task<(bool Ok, T? Result)> SendRequest<T>(string method, object? body = null,
        JsonSerializerOptions? options = null)
    {
        try
        {
            await ApplyRateLimit();

            return (true, await _bot.Client.CallAsync<T>(method, body, options));
        }
        catch (Exception ex)
        {
            await _bot.AddException(ex);
            return (false, default);
        }
    }
}