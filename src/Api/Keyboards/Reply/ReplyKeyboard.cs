namespace TgCore.Api.Keyboards.Reply;

public sealed class ReplyKeyboard : IKeyboardMarkup
{
    [JsonPropertyName("keyboard")]
    public ReplyButton[][] Buttons { get; }
    
    [JsonPropertyName("input_field_placeholder")]
    public string? Placeholder { get; set; }

    [JsonPropertyName("resize_keyboard")]
    public bool ResizeKeyboard { get; set; } = true;

    [JsonPropertyName("one_time_keyboard")]
    public bool OneTimeKeyboard { get; set; } = true;

    public ReplyKeyboard(ReplyButton[][] buttons) => Buttons = buttons;

    public static ReplyKeyboardBuilder Create(
        string? placeholder = null, 
        bool resizeKeyboard = true, 
        bool oneTimeKeyboard = true)
    {
        return new ReplyKeyboardBuilder(placeholder, resizeKeyboard, oneTimeKeyboard);
    }
}