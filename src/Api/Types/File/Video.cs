using TgCore.Api.Types.Base;

namespace TgCore.Api.Types.File;

public sealed class Video : FileBase
{
    [JsonPropertyName("width")]
    public int Width { get; set; }
    
    [JsonPropertyName("height")]
    public int Height { get; set; }
    
    [JsonPropertyName("duration")]
    public int Duration { get; set; }
    
    [JsonPropertyName("thumb")]
    public PhotoSize? Thumbnail { get; set; }
    
    [JsonPropertyName("cover")]
    public PhotoSize[]? Cover { get; set; }
    
    [JsonPropertyName("start_timestamp")]
    public int? StartTimestamp { get; set; }
}