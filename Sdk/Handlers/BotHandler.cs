using TgCore.Sdk.Data.Context;

namespace TgCore.Sdk.Handlers;

public abstract class BotHandler<TContext> where TContext : BotContext
{
    public abstract bool CanHandle(TContext ctx);
    public abstract Task Handle(TContext ctx);
}