using TgCore.Api.Bot;
using TgCore.Sdk.Data.Context;
using TgCore.Sdk.Execution;

namespace TgCore.Sdk.Dispatching;

public class BotActionDispatcher<TContext>  where TContext : BotContext
{
    private readonly List<BotAction<TContext>> _actions;
    private readonly TelegramBot? _bot;

    public BotActionDispatcher(List<BotAction<TContext>> actions,  TelegramBot? bot = null)
    {
        _actions = actions;
        _bot = bot;
    }

    public async Task Execute(string actionName, TContext ctx)
    {
        try
        {
            var action = FindAction(actionName);
            if (action != null)
                await action.Execute(ctx);
            else
                await OnNotFound(ctx);
        }
        catch (Exception ex)
        {
            await OnError(ex, ctx);
        }
    }

    private BotAction<TContext>? FindAction(string actionName)
    {
        return _actions.FirstOrDefault(a => a.ActionName == actionName);
    }

    protected virtual Task OnNotFound(TContext ctx) { return Task.CompletedTask; }

    protected virtual async Task OnError(Exception ex, TContext ctx)
    {
        if (_bot != null)
            await _bot.AddException(ex);
    }
}

