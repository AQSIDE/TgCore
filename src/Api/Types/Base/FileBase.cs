namespace TgCore.Api.Types.Base;

public abstract class FileBase
{
    [JsonPropertyName("file_id")]
    public string FileId { get; set; } = string.Empty;
    
    [JsonPropertyName("file_unique_id")]
    public string FileUniqueId { get; set; } = string.Empty;
    
    [JsonPropertyName("file_name")]
    public string? FileName { get; set; }
    
    [JsonPropertyName("file_size")]
    public long? FileSize { get; set; }
    
    [JsonPropertyName("mime_type")]
    public string? MimeType { get; set; }
}