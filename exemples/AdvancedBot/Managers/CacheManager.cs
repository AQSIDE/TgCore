using AdvancedBot.Data;
using TgCore.Sdk.Managers;

namespace AdvancedBot.Managers;

public static class CacheManager
{
    public static BotDataManager<long, UserProfile> Users { get; } = new();
}