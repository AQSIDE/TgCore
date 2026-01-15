using TgCore.Sdk.Interfaces;

namespace TgCore.Sdk.Resources;

public class BotResources
{
    private readonly Dictionary<Type, object> _modules = new();
    private readonly Lock _lock = new();
    
    public int Count 
    {
        get
        {
            lock (_lock) { return _modules.Count; }
        }
    }

    public BotResources Register<TModule>(TModule module, bool replace = false)
        where TModule : IResourceModule
    {
        if (module is null)
            throw new ArgumentNullException(nameof(module));

        lock (_lock)
        {
            var type = typeof(TModule);

            if (_modules.ContainsKey(typeof(TModule)) && !replace)
                throw new InvalidOperationException($"Module of type {type.Name} is already registered");

            _modules[type] = module;
        }
        
        return this;
    }

    public bool Unregister<TModule>()
        where TModule : class, IResourceModule
    {
        lock (_lock)
        {
            return _modules.Remove(typeof(TModule));
        }
    }

    public TModule Get<TModule>() where TModule : class, IResourceModule
    {
        lock (_lock)
        {
            var type = typeof(TModule);

            if (!_modules.TryGetValue(type, out var module))
                throw new InvalidOperationException($"Module of type {type.Name} is not registered");

            return (TModule)module;
        }
    }

    public bool TryGet<TModule>(out TModule? module) where TModule : class, IResourceModule
    {
        lock (_lock)
        {
            var type = typeof(TModule);

            if (_modules.TryGetValue(type, out var obj))
            {
                module = (TModule)obj;
                return true;
            }

            module = default;
            return false;
        }
    }

    public bool IsRegistered<TModule>() where TModule : class, IResourceModule
    {
        lock (_lock)
        {
            return _modules.ContainsKey(typeof(TModule));
        }
    }

    public void Clear()
    {
        lock (_lock)
        {
            _modules.Clear();
        }
    }
}