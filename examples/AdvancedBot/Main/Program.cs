using AdvancedBot.Factories;
using AdvancedBot.Managers;
using TgCore.Api.Bot;
using TgCore.Api.Clients;
using TgCore.Api.Data;
using TgCore.Api.Enums;
using TgCore.Api.Modules;
using TgCore.Api.Runtime;
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
        var client = new TelegramClient("YOUR_BOT_TOKEN");
        
        _bot = TelegramBot
            .Create(client)
            .UseUpdateReceiver(new LongPollingReceiver(client, 
                new [] { UpdateType.Message , UpdateType.CallbackQuery}, 
                limit:100, 
                timeout:30, 
                startOffset:0))
            .UseLoopRunner(new BotLoopRunner())
            .UseDefaultParseMode(ParseMode.MarkdownV2)
            .UseLifetime()
            .UseRateLimit(new RateLimitModule(
                requestsPerSecond:20, 
                maxBurstSize:25))
            .UseTemporaryMessageLimiter(new TemporaryMessageLimiterModule(
                    maxMessageLimit:3, 
                    mode:TemporaryLimiterMode.Reject))
            .Build();

        _bot.AddUpdateHandler(UpdateHandler)
            .AddErrorHandler(ErrorHandler);

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
        Debug.Console.LogError(ex.ToString());
    }
}