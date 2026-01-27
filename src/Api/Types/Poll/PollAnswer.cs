namespace TgCore.Api.Types.Poll;

public sealed class PollAnswer
{
    [JsonPropertyName("poll_id")]
    public string PollId { get; set; } = string.Empty;
    
    [JsonPropertyName("voter_chat")]
    public Chat? VoterChat { get; set; }
    
    [JsonPropertyName("user")]
    public User? User { get; set; }
    
    [JsonPropertyName("option_ids")]
    public int[] OptionIds { get; set; }
}