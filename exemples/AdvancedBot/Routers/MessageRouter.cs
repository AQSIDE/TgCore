using AdvancedBot.Data.Context;
using TgCore.Api.Clients;
using TgCore.Sdk.Execution;
using TgCore.Sdk.Routing;

namespace AdvancedBot.Routers;

public class MessageRouter : BotHandlerRouter<UserContext>
{
    public MessageRouter(List<BotHandler<UserContext>> handlers, TelegramBot? bot = null) : base(handlers, bot)
    {
        
    }

    public override bool CanRoute(UserContext ctx)
    {
        throw new NotImplementedException();
    }

    protected override async Task OnNotFound(UserContext ctx)
    {
        if (ctx.Bot == null) return;
        
        await ctx.Bot.Message.SendText(
            ctx.User.Id, 
            "Sorry, i dont understand what you said", 
            replyId:ctx.MessageId, 
            lifeTime:TimeSpan.FromSeconds(5));
    }
    
    protected override async Task OnError(UserContext ctx, Exception ex)
    {
        if (ctx.Bot == null) return;

        await ctx.Bot.AddException(ex, ctx.Update);
    }
}