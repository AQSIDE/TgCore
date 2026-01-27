using TgCore.Api.Types.Base;

namespace TgCore.Api.Types.File;

public sealed class Sticker : FileBase
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;
    
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
}