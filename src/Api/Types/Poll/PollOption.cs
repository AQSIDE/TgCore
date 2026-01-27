namespace TgCore.Api.Types.Poll;

public sealed class PollOption
{
    [JsonPropertyName("text")]
    public string Text { get; set; } = string.Empty;
    
    [JsonPropertyName("text_entities")]
    public MessageEntity[]? TextEntities { get; set; }
    
    [JsonPropertyName("voter_count")]
    public int VoterCount { get; set; }
}