namespace TgCore.Api.Types;

public sealed class Story
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("chat")] 
    public Chat Chat { get; set; } = null!;
}