using TgCore.Api.Types.Base;

namespace TgCore.Api.Types.File;

public sealed class Document : FileBase
{
    [JsonPropertyName("thumb")]
    public PhotoSize? Thumbnail { get; set; }
}