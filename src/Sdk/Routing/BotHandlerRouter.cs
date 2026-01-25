using TgCore.Api.Bot;
using TgCore.Sdk.Data.Context;
using TgCore.Sdk.Execution;
using TgCore.Sdk.Interfaces;

namespace TgCore.Sdk.Routing;

public abstract class BotHandlerRouter<TContext> : IBotRouter<TContext> where TContext : BotContext
{
    private readonly List<BotHandler<TContext>> _handlers;
    private readonly TelegramBot? _bot;

    protected BotHandlerRouter(List<BotHandler<TContext>> handlers, TelegramBot? bot = null)
    {
        _handlers = handlers;
        _bot = bot;
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

    protected virtual async Task OnError(TContext ctx, Exception ex)
    {
        if (_bot != null)
            await _bot.AddException(ex);
    }
}