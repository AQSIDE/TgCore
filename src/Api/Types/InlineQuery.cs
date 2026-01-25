namespace TgCore.Api.Types;

public sealed class InlineQuery
{
    [JsonPropertyName("id")]
    public string Id { get; set; }
    
    [JsonPropertyName("from")]
    public User From { get; set; }
    
    [JsonPropertyName("chat")]
    public Chat? Chat { get; set; }
    
    [JsonPropertyName("query")]
    public string Query { get; set; } 

    [JsonPropertyName("offset")]
    public string Offset { get; set; } 

    [JsonPropertyName("chat_type")]
    public string? ChatType { get; set; }
}