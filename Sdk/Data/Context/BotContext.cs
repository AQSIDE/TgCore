using TgCore.Api.Clients;
using TgCore.Api.Enums;
using TgCore.Api.Types;

namespace TgCore.Sdk.Data.Context;

public abstract class BotContext
{
    public Update Update { get; }
    public TelegramBot? Bot { get; }
    
    public User? GetFrom => Update.GetFrom;
    public Chat? GetChat => Update.GetChat;
    public Message? GetMessage => Update.GetMessage;
    public UpdateType UpdateType => Update.Type;
    
    public long? FromId => Update.FromId;
    public long? ChatId => Update.ChatId;
    public long? FromOrChatId => Update.FromOrChatId;
    public long? MessageId => Update.MessageId;
    public string? CallbackData => Update.CallbackData;
    public string? Text => Update.Text;

    protected BotContext(Update update, TelegramBot? bot = null)
    {
        Update = update;
        Bot = bot;
    }
}