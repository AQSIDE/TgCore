namespace TgCore.Api.Types.Poll;

public sealed class InputPollOption
{
    [JsonPropertyName("text")]
    public string Text { get; set; }
    
    [JsonPropertyName("text_parse_mode")]
    public string ParseMode { get; set; }

    public InputPollOption(string text, string parseMode)
    {
        Text = text;
        ParseMode = parseMode;
    }

    public static InputPollOption Create(string text, ParseMode parseMode = Enums.ParseMode.None)
    {
        return new InputPollOption(text, BotHelper.GetParseModeName(parseMode));
    }
}