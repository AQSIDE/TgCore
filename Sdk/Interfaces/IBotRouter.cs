namespace TgCore.Sdk.Interfaces;

public interface IBotRouter<in TContext>
{
    Task Route(TContext ctx);
    bool CanRoute(TContext ctx);
}