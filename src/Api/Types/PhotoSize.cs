namespace TgCore.Api.Types;

public sealed class PhotoSize
{
    [JsonPropertyName("file_id")]
    public string FileId { get; set; } = default;
    
    [JsonPropertyName("file_unique_id")]
    public string FileUniqueId { get; set; } = default;
    
    [JsonPropertyName("width")]
    public int Width { get; set; }
    
    [JsonPropertyName("height")]
    public int Height { get; set; }
    
    [JsonPropertyName("file_size")]
    public long? FileSize { get; set; }
}