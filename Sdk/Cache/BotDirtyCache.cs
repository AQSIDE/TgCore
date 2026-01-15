namespace TgCore.Sdk.Cache;

public class BotDirtyCache<TKey, TValue> : BotCache<TKey, TValue>
{
    public void MarkDirty(TValue value, Func<TValue, TKey> keySelector)
    {
        if (value != null)
            AddOrUpdate(keySelector(value), value);
    }

    public List<TValue> Drain()
    {
        var drained = new List<TValue>();
        foreach (var kv in Storage.ToArray())
        {
            if (Remove(kv.Key, out var val) && val != null)
                drained.Add(val);
        }
        return drained;
    }
    
    public List<TValue> Peek()
    {
        return Storage.Values.ToList();
    }
    
    public bool TryPeek(TKey key, out TValue? value)
    {
        return Storage.TryGetValue(key, out value);
    }
}