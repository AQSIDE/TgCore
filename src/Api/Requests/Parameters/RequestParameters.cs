namespace TgCore.Api.Requests.Parameters;

public abstract class RequestParameters
{
    public string? BusinessConnectionId { get; set; }
    public long? MessageThreadId { get; set; }
    
    public abstract Dictionary<string, object> ToDictionary();

    protected Dictionary<string, object> GetBase()
    {
        return new TelegramParametersBuilder()
            .Add("business_connection_id", BusinessConnectionId)
            .Add("message_thread_id", MessageThreadId)
            .Build();
    }
}