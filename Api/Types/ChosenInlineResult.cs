using System.Text.Json.Serialization;

namespace TgCore.Api.Types;

public class ChosenInlineResult
{
    [JsonPropertyName("result_id")]
    public string ResultId { get; set; } 

    [JsonPropertyName("from")]
    public User From { get; set; }
    
    [JsonPropertyName("chat")]
    public Chat? Chat { get; set; }

    [JsonPropertyName("inline_message_id")]
    public string? InlineMessageId { get; set; } 

    [JsonPropertyName("query")]
    public string Query { get; set; } 
}