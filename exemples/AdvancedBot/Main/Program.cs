using AdvancedBot.Factories;
using AdvancedBot.Managers;
using TgCore.Api.Bot;
using TgCore.Api.Clients;
using TgCore.Api.Data;
using TgCore.Api.Modules;
using TgCore.Api.Types;
using TgCore.Diagnostics.Debugger;

namespace AdvancedBot.Main;

public class Program
{
    private static ContextFactory _contextFactory = null!;
    private static BuildFactory _buildFactory = null!;
    private static RouterManager _routerManager = null!;

    private static TelegramBot _bot = null!;

    private static async Task Main()
    {
        _bot = new TelegramBot(new BotOptions("YOUR_BOT_TOKEN"));

        _bot.Options.RateLimit = new RateLimitModule();
        _bot.Options.Lifetime = new LifetimeModule(_bot, _bot.MainLoop);
        _bot.Options.TemporaryMessageLimiter = new TemporaryMessageLimiterModule(3);
        
        _bot.AddUpdateHandler(UpdateHandler);
        _bot.AddErrorHandler(ErrorHandler);

        _buildFactory = new BuildFactory(_bot);
        _contextFactory = new ContextFactory(_bot);

        _routerManager = _buildFactory.BuildRoute();

        await _bot.Run();
    }

    private static async Task UpdateHandler(Update update)
    {
        var ctx = _contextFactory.CreateContext(update);
        if (ctx == null) return;

        await _routerManager.Route(ctx);
    }

    private static async Task ErrorHandler(Exception ex)
    {
        Debug.LogError(ex.ToString());
    }
}