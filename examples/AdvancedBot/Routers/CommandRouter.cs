using AdvancedBot.Data.Context;
using TgCore.Api.Bot;
using TgCore.Api.Clients;
using TgCore.Sdk.Execution;
using TgCore.Sdk.Routing;

namespace AdvancedBot.Routers;

public class CommandRouter : BotHandlerRouter<UserContext>
{
    public CommandRouter(List<BotHandler<UserContext>> handlers, TelegramBot? bot = null) : base(handlers, bot)
    {
    }

    public override bool CanRoute(UserContext ctx)
    {
        if (ctx.Update.Message?.Text == null) return false;
        
        return ctx.Update.Message.Text.StartsWith("/");
    }

    protected override async Task OnNotFound(UserContext ctx)
    {
        if (ctx.Bot == null) return;
        
        await ctx.Bot.Requests.SendText(
            ctx.User.Id, 
            "Unknown command", 
            replyId:ctx.MessageId, 
            lifeTime:TimeSpan.FromSeconds(5));
    }

    protected override async Task OnError(UserContext ctx, Exception ex)
    {
        if (ctx.Bot == null) return;

        await ctx.Bot.AddException(ex);
    }
}