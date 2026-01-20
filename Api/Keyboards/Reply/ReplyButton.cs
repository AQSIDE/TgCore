namespace TgCore.Api.Keyboards.Reply;

public sealed class ReplyButton
{
    [JsonPropertyName("text")]
    public string Text { get; }

    [JsonPropertyName("request_contact")]
    public bool? RequestContact { get; set; }

    [JsonPropertyName("request_location")]
    public bool? RequestLocation { get; set; }

    public ReplyButton(string text) => Text = text;
    
    public static ReplyButton CreateContact(string text) => new(text) { RequestContact = true };
    public static ReplyButton CreateLocation(string text) => new(text) { RequestLocation = true };
}