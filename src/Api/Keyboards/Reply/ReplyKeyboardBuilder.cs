namespace TgCore.Api.Keyboards.Reply;

public class ReplyKeyboardBuilder
{
    private readonly List<List<ReplyButton>> _rows = new();
    public string? Placeholder { get; }
    public bool ResizeKeyboard { get; }
    public bool OneTimeKeyboard { get; }

    public ReplyKeyboardBuilder(string? placeholder = null, bool resizeKeyboard = true, bool oneTimeKeyboard = true)
    {
        Placeholder = placeholder;
        ResizeKeyboard = resizeKeyboard;
        OneTimeKeyboard = oneTimeKeyboard;
    }

    public ReplyKeyboardBuilder Row(params ReplyButton[] buttons)
    {
        if (buttons.Length == 0) return this;
        
        _rows.Add(buttons.ToList());
        return this;
    }

    public IKeyboardMarkup Build()
    {
        if (_rows.Count == 0)
            return new ReplyKeyboard(Array.Empty<ReplyButton[]>());
        
        var buttons = new ReplyButton[_rows.Count][];
        
        for (int i = 0; i < _rows.Count; i++)
        {
            buttons[i] = _rows[i].ToArray(); 
        }

        var reply = new ReplyKeyboard(buttons)
        {
            Placeholder = Placeholder,
            ResizeKeyboard = ResizeKeyboard,
            OneTimeKeyboard = OneTimeKeyboard
        };

        return reply;
    }
}