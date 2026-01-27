using AdvancedBot.Data.Context;
using TgCore.Api.Bot;
using TgCore.Api.Clients;
using TgCore.Sdk.Interfaces;
using TgCore.Sdk.Managers;

namespace AdvancedBot.Managers;

public class RouterManager : BotRouterManager<UserContext>
{
    public RouterManager(List<IBotRouter<UserContext>> routers, TelegramBot? bot = null) : base(routers, bot)
    {
    }
}