namespace TgCore.Api.Requests.Parameters;

public class TelegramParametersBuilder
{
    private readonly Dictionary<string, object> _parameters = new();

    public TelegramParametersBuilder Add(string key, object? value)
    {
        if (value != null)
        {
            _parameters[key] = value;
        }
        return this;
    }
    
    public TelegramParametersBuilder Add(string key, string? value)
    {
        if (!string.IsNullOrEmpty(value))
        {
            _parameters[key] = value;
        }
        return this;
    }
    
    public TelegramParametersBuilder Add(string key, bool value)
    {
        _parameters[key] = value;
        return this;
    }
    
    public TelegramParametersBuilder AddDictionary(Dictionary<string, object>? other)
    {
        if (other == null) return this;
        
        foreach (var kvp in other)
        {
            Add(kvp.Key, kvp.Value);
        }
        return this;
    }
    
    public Dictionary<string, object> Build()
    {
        return _parameters;
    }
    
    public TelegramParametersBuilder Clear()
    {
        _parameters.Clear();
        return this;
    }
}