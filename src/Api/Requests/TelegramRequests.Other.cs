using TgCore.Api.Requests.Parameters;
using TgCore.Api.Systems.Telemetry.Data;

namespace TgCore.Api.Requests;

public partial class TelegramRequests
{
    public async Task<RequestResponse<User>> GetMe(CancellationToken ct = default)
    {
        return await SendRequest<User>(TelegramMethods.GET_ME, null, ct:ct);
    }
    
    public async Task<RequestResponse<ChatFullInfo>> GetChat(CancellationToken ct = default)
    {
        return await SendRequest<ChatFullInfo>(TelegramMethods.GET_CHAT, body: null, ct: ct);
    }
    
    public async Task<RequestResponse<Message>> SendChatAction(
        long chatId,
        string action,
        ShortParameters? shortParameters = null, 
        CancellationToken ct = default)
    {
        var parameters = new TelegramParametersBuilder()
            .Add("chat_id", chatId)
            .Add("action",action)
            .AddDictionary(shortParameters?.ToDictionary())
            .Build();
        
        return await SendRequest<Message>(TelegramMethods.SEND_DICE, parameters, ct);
    }
}