using TgCore.Api.Clients;
using TgCore.Api.Types;
using TgCore.Sdk.Data.Context;

namespace AdvancedBot.Data.Context;

public class UserContext : BotContext
{
    public UserProfile User { get; }
    
    public UserContext(Update update, UserProfile user, TelegramBot? bot = null) : base(update, bot)
    {
        User = user;
    }
}