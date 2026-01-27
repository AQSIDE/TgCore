namespace TgCore.Api.Types;

public sealed class InlineQuery
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;
    
    [JsonPropertyName("from")]
    public User From { get; set; } = null!;
    
    [JsonPropertyName("chat")]
    public Chat? Chat { get; set; }
    
    [JsonPropertyName("query")]
    public string Query { get; set; } = string.Empty;

    [JsonPropertyName("offset")]
    public string Offset { get; set; } = string.Empty;

    [JsonPropertyName("chat_type")]
    public string? ChatType { get; set; }
}