namespace TgCore.Api.Types.Business;

public class BusinessMessagesDeleted
{
    [JsonPropertyName("business_connection_id")]
    public string Id { get; set; } = string.Empty;
    
    [JsonPropertyName("chat")]
    public Chat Chat { get; set; } = null!;
    
    [JsonPropertyName("message_ids")]
    public int[]? MessageIds { get; set; }
}