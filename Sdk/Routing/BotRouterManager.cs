using TgCore.Sdk.Data.Context;

namespace TgCore.Sdk.Routing;

public class BotRouterManager<TContext> where TContext : BotContext
{
    private readonly List<BotRouter<TContext>> _routers;

    protected BotRouterManager(List<BotRouter<TContext>> routers)
    {
        _routers = routers;
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

    private BotRouter<TContext>? FindRouter(TContext ctx)
        => _routers.FirstOrDefault(router => router.CanRoute(ctx));
    
    protected virtual Task OnNotFound(TContext ctx) => Task.CompletedTask;
    protected virtual Task OnError(TContext ctx, Exception ex) => Task.CompletedTask;
}