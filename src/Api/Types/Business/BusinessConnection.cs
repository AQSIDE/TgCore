namespace TgCore.Api.Types.Business;

public sealed class BusinessConnection
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("user")] 
    public User User { get; set; } = null!;
    
    [JsonPropertyName("user_chat_id")] 
    public long UserChatId { get; set; }
    
    [JsonPropertyName("date")] 
    public int Date { get; set; }
    
    [JsonPropertyName("rights")] 
    public BusinessBotRights? Rights { get; set; }
    
    [JsonPropertyName("is_enabled")] 
    public bool IsEnabled { get; set; }
}