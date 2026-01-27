using AdvancedBot.Data.Context;
using TgCore.Api.Bot;
using TgCore.Api.Clients;
using TgCore.Sdk.Execution;
using TgCore.Sdk.Routing;

namespace AdvancedBot.Routers;

public class CallbackRouter : BotHandlerRouter<UserContext>
{
    public CallbackRouter(List<BotHandler<UserContext>> handlers, TelegramBot? bot = null) : base(handlers, bot)
    {
    }

    public override bool CanRoute(UserContext ctx)
    {
        return ctx.Update.CallbackQuery != null;
    }
    
    protected override async Task OnError(UserContext ctx, Exception ex)
    {
        if (ctx.Bot == null) return;

        await ctx.Bot.AddException(ex);
    }
}