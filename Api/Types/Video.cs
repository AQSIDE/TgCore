namespace TgCore.Api.Types;

public sealed class Video
{
    [JsonPropertyName("file_id")]
    public string FileId { get; set; } = default;
    
    [JsonPropertyName("file_unique_id")]
    public string FileUniqueId { get; set; } = default;
    
    [JsonPropertyName("width")]
    public int Width { get; set; }
    
    [JsonPropertyName("height")]
    public int Height { get; set; }
    
    [JsonPropertyName("duration")]
    public int Duration { get; set; }
    
    [JsonPropertyName("thumb")]
    public PhotoSize? Thumbnail { get; set; }
    
    [JsonPropertyName("file_name")]
    public string? FileName { get; set; }
    
    [JsonPropertyName("mime_type")]
    public string? MimeType { get; set; }
    
    [JsonPropertyName("file_size")]
    public long? FileSize { get; set; }
}