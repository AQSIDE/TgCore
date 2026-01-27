using AdvancedBot.Data;
using AdvancedBot.Data.Context;
using AdvancedBot.Managers;
using TgCore.Api.Bot;
using TgCore.Api.Types;
using TgCore.Diagnostics.Debugger;

namespace AdvancedBot.Factories;

public class ContextFactory
{
    private readonly TelegramBot _bot;

    public ContextFactory(TelegramBot bot)
    {
        _bot = bot;
    }
    
    public UserContext? CreateContext(Update update)
    {
        var user = GetOrCreateUser(update);
        return user != null ? new UserContext(update, user, _bot) : null;
    }

    private UserProfile? GetOrCreateUser(Update update)
    {
        var from = update.GetFrom;
        if (from == null) return null;

        if (CacheManager.Users.TryGet(from.Id, out var user))
            return user;

        user = CreateNewUser(update);
        CacheManager.Users.AddOrUpdate(user.Id, user);

        Debug.Console.LogInfo($"New profile: {user.Username}, {user.Id}", new LogOptions { Category = "GetOrCreateUser"});
        return user;
    }
    
    private UserProfile CreateNewUser(Update update)
    {
        return new UserProfile(
            update.FromId!.Value, 
            update.GetFrom!.FirstName ?? update.GetFrom.Username!);
    }
}