using TgCore.Sdk.Data.Context;

namespace TgCore.Sdk.Execution;

public abstract class BotAction<TContext> where TContext : BotContext
{
    public string ActionName { get; set; }

    protected BotAction(string actionName)
    {
        ActionName = actionName;
    }
    
    public abstract Task Execute(TContext ctx);
}