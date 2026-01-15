using TgCore.Sdk.Data.Context;
using TgCore.Sdk.Handlers;

namespace TgCore.Sdk.Routing;

public abstract class BotRouter<TContext> where TContext : BotContext
{
    private readonly List<BotHandler<TContext>> _handlers;

    protected BotRouter(List<BotHandler<TContext>> handlers)
    {
        _handlers = handlers;
    }

    public async Task Route(TContext ctx)
    {
        try
        {
            var router = FindHandler(ctx);

            if (router != null)
                await router.Handle(ctx);
            else
                await OnNotFound(ctx);
        }
        catch (Exception ex)
        {
            await OnError(ctx, ex);
        }
    }

    private BotHandler<TContext>? FindHandler(TContext ctx)
        => _handlers.FirstOrDefault(handler => handler.CanHandle(ctx));

    public abstract bool CanRoute(TContext ctx);

    protected virtual Task OnNotFound(TContext ctx) => Task.CompletedTask;
    protected virtual Task OnError(TContext ctx, Exception ex) => Task.CompletedTask;
}