namespace TgCore.Api.Types;

public sealed class Message
{
    [JsonPropertyName("message_id")]
    public long Id { get; set; }
    
    [JsonPropertyName("chat")]
    public Chat? Chat { get; set; }
    
    [JsonPropertyName("reply_to_message")]
    public Message? ReplyToMessage { get; set; }
    
    [JsonPropertyName("text")]
    public string? Text { get; set; }
    
    [JsonPropertyName("from")]
    public User? From { get; set; }
    
    [JsonPropertyName("date")]
    public long Date { get; set; }
    
    [JsonPropertyName("edit_date")]
    public long? EditDate { get; set; }
    
    public bool IsEmpty => string.IsNullOrEmpty(Text);
    public bool IsReply => ReplyToMessage != null;
}