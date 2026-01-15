using TgCore.Sdk.Cache;

namespace TgCore.Sdk.Managers;

public class BotDataManager<TKey, TValue> where TKey : notnull
{
    protected readonly BotCache<TKey, TValue> Cache = new();

    public virtual void AddOrUpdate(TKey key, TValue value)
    {
        Cache.AddOrUpdate(key, value);
    }

    public virtual bool Remove(TKey key)
    {
        return Cache.Remove(key, out _);
    }
    
    public virtual bool TryRemove(TKey key, out TValue? value)
    {
        return Cache.Remove(key, out value);
    }
    
    public virtual TValue Get(TKey key)
    {
        Cache.TryGet(key, out var value);
        return value;
    }

    public virtual bool TryGet(TKey key, out TValue? value)
    {
        return Cache.TryGet(key, out value);
    }
    
    public virtual IEnumerable<TValue> GetAll()
    {
        return Cache.Storage.Values;
    }
}