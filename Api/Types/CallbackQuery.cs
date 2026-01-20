namespace TgCore.Api.Types;

public sealed class CallbackQuery
{
    [JsonPropertyName("id")] 
    public string Id { get; init; }

    [JsonPropertyName("from")] 
    public User From { get; init; }
    
    [JsonPropertyName("chat")]
    public Chat? Chat { get; set; }
    
    [JsonPropertyName("message")] 
    public Message? Message { get; init; }

    [JsonPropertyName("data")] 
    public string? Data { get; init; }
}