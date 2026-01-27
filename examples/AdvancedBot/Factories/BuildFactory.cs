using AdvancedBot.Data.Context;
using AdvancedBot.Handlers;
using AdvancedBot.Managers;
using AdvancedBot.Routers;
using TgCore.Api.Bot;
using TgCore.Sdk.Execution;
using TgCore.Sdk.Interfaces;

namespace AdvancedBot.Factories;

public class BuildFactory
{
    private readonly TelegramBot _bot;

    public BuildFactory(TelegramBot bot)
    {
        _bot = bot;
    }
    
    public RouterManager BuildRoute()
    {
        var routers = new List<IBotRouter<UserContext>>();

        // CommandRouter
        var commandRouterHandlers = new List<BotHandler<UserContext>>();
        commandRouterHandlers.Add(new CommandHandler());
        routers.Add(new CommandRouter(commandRouterHandlers, _bot));

        // CallbackRouter
        var callbackRouterHandlers = new List<BotHandler<UserContext>>();
        callbackRouterHandlers.Add(new CallbackHandler());
        routers.Add(new CallbackRouter(callbackRouterHandlers, _bot));

        //MessageRouter
        var messageRouterHandlers = new List<BotHandler<UserContext>>();
        messageRouterHandlers.Add(new MessageHandler());
        routers.Add(new MessageRouter(messageRouterHandlers, _bot));

        return new RouterManager(routers, _bot);
    }
}