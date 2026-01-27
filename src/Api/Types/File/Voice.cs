using TgCore.Api.Types.Base;

namespace TgCore.Api.Types.File;

public sealed class Voice : FileBase
{
    [JsonPropertyName("duration")]
    public int Duration { get; set; }
}