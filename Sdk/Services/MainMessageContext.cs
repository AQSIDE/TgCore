using TgCore.Api.Interfaces;
using TgCore.Api.Types;

namespace TgCore.Sdk.Services;

public class MainMessageContext
{
    public Message Message { get; }
    public IKeyboardMarkup? Keyboard { get; }

    public MainMessageContext(Message message, IKeyboardMarkup? keyboard = null)
    {
        Message = message;
        Keyboard = keyboard;
    }
}