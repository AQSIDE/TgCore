using TgCore.Api.Types.Base;

namespace TgCore.Api.Types.File;

public sealed class VideoNote : FileBase
{
    [JsonPropertyName("length")]
    public int Length { get; set; }
    
    [JsonPropertyName("duration")]
    public int Duration { get; set; }
    
    [JsonPropertyName("thumbnail")]
    public PhotoSize? Thumbnail { get; set; }
}