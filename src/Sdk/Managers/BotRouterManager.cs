using TgCore.Sdk.Data.Context;
using TgCore.Sdk.Interfaces;

namespace TgCore.Sdk.Managers;

public class BotRouterManager<TContext> where TContext : BotContext
{
    private readonly List<IBotRouter<TContext>> _routers;
    private readonly TelegramBot? _bot;

    protected BotRouterManager(List<IBotRouter<TContext>> routers, TelegramBot? bot = null)
    {
        _routers = routers;
        _bot = bot;
    }

    public async Task Route(TContext ctx)
    {
        try
        {
            var router = FindRouter(ctx);

            if (router != null)
                await router.Route(ctx);
            else
                await OnNotFound(ctx);
        }
        catch (Exception ex)
        {
            await OnError(ctx, ex);
        }
    }

    private IBotRouter<TContext>? FindRouter(TContext ctx)
        => _routers.FirstOrDefault(router => router.CanRoute(ctx));
    
    protected virtual Task OnNotFound(TContext ctx) => Task.CompletedTask;
    protected virtual async Task OnError(TContext ctx, Exception ex)
    {
        if (_bot != null)
            await _bot.AddException(ex, null);
    }
}