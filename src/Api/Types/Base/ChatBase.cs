namespace TgCore.Api.Types.Base;

public abstract class ChatBase
{
    [JsonPropertyName("id")]
    public long Id { get; set; }
    
    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;
    
    [JsonPropertyName("title")]
    public string? Title { get; set; }

    [JsonPropertyName("username")]
    public string? Username { get; set; }

    [JsonPropertyName("first_name")]
    public string? FirstName { get; set; }

    [JsonPropertyName("last_name")]
    public string? LastName { get; set; }

    [JsonPropertyName("is_forum")]
    public bool? IsForum { get; set; }
    
    [JsonPropertyName("is_direct_message")]
    public bool? IsDirectMessage { get; set; }
}