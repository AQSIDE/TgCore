using AdvancedBot.Data.Context;
using TgCore.Sdk.Execution;

namespace AdvancedBot.Handlers;

public class MessageHandler : BotHandler<UserContext>
{
    public override bool CanHandle(UserContext ctx)
    {
        throw new NotImplementedException();
    }

    public override Task Handle(UserContext ctx)
    {
        throw new NotImplementedException();
    }
}