using TgCore.Api.Interfaces;

namespace TgCore.Api.Keyboards.Inline;

public sealed class InlineKeyboardBuilder
{
    private readonly List<List<InlineButton>> _rows = new();

    public InlineKeyboardBuilder Row(params InlineButton[] buttons)
    {
        if (buttons.Length == 0) return this;
        
        _rows.Add(buttons.ToList());
        return this;
    }

    public IKeyboardMarkup Build()
    {
        if (_rows.Count == 0)
            return new InlineKeyboard(Array.Empty<InlineButton[]>());
        
        var buttons = new InlineButton[_rows.Count][];
        
        for (int i = 0; i < _rows.Count; i++)
        {
            buttons[i] = _rows[i].ToArray(); 
        }

        return new InlineKeyboard(buttons);
    }
}