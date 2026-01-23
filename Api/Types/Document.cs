namespace TgCore.Api.Types;

public class Document
{
    [JsonPropertyName("file_id")]
    public string FileId { get; set; } = default;
    
    [JsonPropertyName("file_unique_id")]
    public string FileUniqueId { get; set; } = default;
    
    [JsonPropertyName("thumb")]
    public PhotoSize? Thumbnail { get; set; }
    
    [JsonPropertyName("file_name")]
    public string? FileName { get; set; }
    
    [JsonPropertyName("mime_type")]
    public string? MimeType { get; set; }
    
    [JsonPropertyName("file_size")]
    public long? FileSize { get; set; }
}