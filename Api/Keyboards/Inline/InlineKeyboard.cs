namespace TgCore.Api.Keyboards.Inline;

public sealed class InlineKeyboard : IKeyboardMarkup
{
    [JsonPropertyName("inline_keyboard")]
    public InlineButton[][] Buttons { get; } 

    public InlineKeyboard(InlineButton[][] buttons) => Buttons = buttons;
    
    public static IKeyboardMarkup Empty => new InlineKeyboard(Array.Empty<InlineButton[]>());

    public static InlineKeyboardBuilder Create() => new();
}