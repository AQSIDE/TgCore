using TgCore.Api.Types.Base;

namespace TgCore.Api.Types.File;

public sealed class Audio : FileBase
{
    [JsonPropertyName("duration")]
    public int Duration { get; set; }
    
    [JsonPropertyName("performer")]
    public string? Performer { get; set; }
    
    [JsonPropertyName("title")]
    public string? Title { get; set; }
    
    [JsonPropertyName("thumb")]
    public PhotoSize? Thumbnail { get; set; }
}