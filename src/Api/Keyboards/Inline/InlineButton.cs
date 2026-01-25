namespace TgCore.Api.Keyboards.Inline;

public sealed class InlineButton
{
    [JsonPropertyName("text")]
    public string Text { get; }
    
    [JsonPropertyName("callback_data")]
    public string? Data { get; }
    
    [JsonPropertyName("url")]
    public string? Url { get; }

    [JsonConstructor]
    private InlineButton(string text, string? data, string? url)
    {
        Text = text;
        Data = data;
        Url = url;
    }

    public static InlineButton CreateData(string text, string data) => new (text, data, null);
    public static InlineButton CreateUrl(string text, string url) => new(text, null, url);
}