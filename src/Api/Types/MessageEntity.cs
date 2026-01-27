namespace TgCore.Api.Types;

public sealed class MessageEntity
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;
    
    [JsonPropertyName("url")]
    public string Url { get; set; } = string.Empty;
    
    [JsonPropertyName("offset")]
    public int Offset { get; set; }
    
    [JsonPropertyName("length")]
    public int Length { get; set; }
    
    [JsonPropertyName("user")]
    public User? User { get; set; }
    
    [JsonPropertyName("language")]
    public string? Language { get; set; }
    
    [JsonPropertyName("custom_emoji_id")]
    public string? CustomEmojiId { get; set; }
}