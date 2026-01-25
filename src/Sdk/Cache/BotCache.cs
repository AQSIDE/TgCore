using System.Collections.Concurrent;

namespace TgCore.Sdk.Cache;

public class BotCache<TKey, TValue>
{
    private readonly ConcurrentDictionary<TKey, TValue> _storage = new();

    public IReadOnlyDictionary<TKey, TValue> Storage => _storage;

    public void AddOrUpdate(TKey key, TValue value) =>
        _storage.AddOrUpdate(key, value, (_, __) => value);

    public bool TryGet(TKey key, out TValue? value) =>
        _storage.TryGetValue(key, out value);

    public bool Remove(TKey key, out TValue? removed) =>
        _storage.TryRemove(key, out removed);

    public void Clear() => _storage.Clear();
}