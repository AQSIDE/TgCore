namespace TgCore.Api.Types;

public sealed class Sticker
{
    [JsonPropertyName("file_id")]
    public string FileId { get; set; } = default;
    
    [JsonPropertyName("file_unique_id")]
    public string FileUniqueId { get; set; } = default;
    
    [JsonPropertyName("type")]
    public string Type { get; set; } = default;
    
    [JsonPropertyName("width")]
    public int Width { get; set; }
    
    [JsonPropertyName("height")]
    public int Height { get; set; }
    
    [JsonPropertyName("is_animated")]
    public bool IsAnimated { get; set; }
    
    [JsonPropertyName("is_video")]
    public bool IsVideo { get; set; }
    
    [JsonPropertyName("emoji")]
    public string? Emoji { get; set; }
    
    [JsonPropertyName("set_name")]
    public string? SetName { get; set; }
    
    [JsonPropertyName("thumb")]
    public PhotoSize? Thumbnail { get; set; }
    
    [JsonPropertyName("file_size")]
    public long? FileSize { get; set; }
}