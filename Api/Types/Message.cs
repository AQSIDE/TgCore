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
    
    [JsonPropertyName("photo")]
    public PhotoSize[]? Photo { get; set; }
    
    [JsonPropertyName("video")]
    public Video? Video { get; set; }
    
    [JsonPropertyName("audio")]
    public Audio? Audio { get; set; }
    
    [JsonPropertyName("document")]
    public Document? Document { get; set; }
    
    [JsonPropertyName("animation")]
    public Animation? Animation { get; set; }
    
    [JsonPropertyName("voice")]
    public Voice? Voice { get; set; }
    
    [JsonPropertyName("sticker")]
    public Sticker? Sticker { get; set; }
    
    [JsonPropertyName("caption")]
    public string? Caption { get; set; }
    
    public bool IsEmpty => string.IsNullOrEmpty(Text) && 
                           Photo == null && 
                           Video == null && 
                           Audio == null && 
                           Document == null && 
                           Animation == null;
    public bool IsReply => ReplyToMessage != null;
}