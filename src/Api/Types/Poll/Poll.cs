namespace TgCore.Api.Types.Poll;

public sealed class Poll
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;
    
    [JsonPropertyName("question")]
    public string Question { get; set; } = string.Empty;
    
    [JsonPropertyName("question_entities")]
    public MessageEntity[]? QuestionEntities { get; set; }
    
    [JsonPropertyName("options")]
    public PollOption[]? Options { get; set; }
    
    [JsonPropertyName("total_voter_count")]
    public int VoterCount { get; set; }
    
    [JsonPropertyName("is_closed")]
    public bool IsClosed { get; set; }
    
    [JsonPropertyName("is_anonymous")]
    public bool IsAnonymous { get; set; }
    
    [JsonPropertyName("type")]
    public string TypeString { get; set; } = string.Empty;
    
    [JsonPropertyName("allows_multiple_answers")]
    public bool AllowsMultipleAnswers { get; set; }
    
    [JsonPropertyName("correct_option_id")]
    public int CorrectOptionId { get; set; }
    
    [JsonPropertyName("explanation")]
    public string Explanation { get; set; } = string.Empty;
    
    [JsonPropertyName("explanation_entities")]
    public MessageEntity[]? ExplanationEntities { get; set; }
    
    [JsonPropertyName("open_period")]
    public int OpenPeriod { get; set; }
    
    [JsonPropertyName("close_date")]
    public int CloseDate { get; set; }

    public PollType Type()
    {
        switch (TypeString)
        {
            case "quiz":
                return PollType.Quiz;
            case "regular":
                return PollType.Regular;
            default:
                throw new Exception("Unknown type");
        }
    }
}