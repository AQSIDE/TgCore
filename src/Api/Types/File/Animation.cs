using TgCore.Api.Types.Base;

namespace TgCore.Api.Types.File;

public sealed class Animation : FileBase
{
    [JsonPropertyName("width")]
    public int Width { get; set; }
    
    [JsonPropertyName("height")]
    public int Height { get; set; }
    
    [JsonPropertyName("duration")]
    public int Duration { get; set; }
    
    [JsonPropertyName("thumb")]
    public PhotoSize? Thumbnail { get; set; }
}