using TgCore.Api.Requests.Parameters;

namespace TgCore.Api.Requests;

public partial class TelegramRequests
{
    public async Task<User?> GetMe()
    {
        try
        {
            await ApplyRateLimit();

            var user = await _bot.Client.CallAsync<User?>(TelegramMethods.GET_ME);
            return user;
        }
        catch (Exception ex)
        {
            await _bot.AddException(ex);
            return null;
        }
    }
    
    public async Task<ChatFullInfo?> GetChat()
    {
        try
        {
            await ApplyRateLimit();

            var info = await _bot.Client.CallAsync<ChatFullInfo?>(TelegramMethods.GET_CHAT);
            return info;
        }
        catch (Exception ex)
        {
            await _bot.AddException(ex);
            return null;
        }
    }
    
    public async Task<Message?> SendChatAction(
        long chatId,
        string action,
        ShortParameters? shortParameters = null)
    {
        try
        {
            await ApplyRateLimit();
            
            var parameters = new TelegramParametersBuilder()
                .Add("chat_id", chatId)
                .Add("action",action)
                .AddDictionary(shortParameters?.ToDictionary())
                .Build();

            var message = await _bot.Client.CallAsync<Message?>(TelegramMethods.SEND_DICE, parameters);

            return message;
        }
        catch (Exception ex)
        {
            await _bot.AddException(ex);
            return null;
        }
    }
}