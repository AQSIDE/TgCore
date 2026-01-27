namespace TgCore.Api.Types;

public sealed class MessageId
{
    [JsonPropertyName("message_id")]
    public object? Id { get; set; }
}