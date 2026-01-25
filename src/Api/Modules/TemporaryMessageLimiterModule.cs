using TgCore.Diagnostics.Debugger;

namespace TgCore.Api.Modules;

public class TemporaryMessageLimiterModule : ITemporaryMessageLimiterModule
{
    private readonly Dictionary<long, List<long>> _messages = new();
    private readonly object _lock = new();
    
    public ILifetimeModule? LifetimeModule { get; }
    public TemporaryLimiterMode Mode { get; set; }
    public int MaxMessageLimit { get; set; }
    public bool UseLogging { get; set; }

    public TemporaryMessageLimiterModule(int maxMessageLimit = 3, TemporaryLimiterMode mode = TemporaryLimiterMode.Reject, ILifetimeModule? lifetimeModule = null)
    {
        MaxMessageLimit = maxMessageLimit;
        LifetimeModule = lifetimeModule;
        Mode = mode;
    }

    public async Task<bool> CanSend(long chatId)
    {
        long? messageToDelete = null;
        lock (_lock)
        {
            if (!_messages.TryGetValue(chatId, out var messages))
                return true;

            if (messages.Count < MaxMessageLimit)
                return true;
            
            switch (Mode)
            {
                case TemporaryLimiterMode.Reject:
                    return false;
                
                case TemporaryLimiterMode.ReplaceOldest:
                    if (LifetimeModule == null)
                    {
                        Debug.LogWarning("ILifetimeModule not set", "TemporaryMessageLimiterModule");
                        return false;
                    }
                    
                    messageToDelete = messages.First();
                    messages.RemoveAt(0);
                    break;
                
                case TemporaryLimiterMode.ReplaceNewest:
                    if (LifetimeModule == null)
                    {
                        Debug.LogWarning("ILifetimeModule not set", "TemporaryMessageLimiterModule");
                        return false;
                    }
                    
                    messageToDelete = messages.Last();
                    messages.RemoveAt(messages.Count - 1);
                    break;
            }
        }
        
        if (messageToDelete.HasValue && LifetimeModule != null)
        {
            await LifetimeModule.Delete(chatId, messageToDelete.Value);
        }
        
        return true;
    }

    public async Task RegisterMessage(long chatId, long messageId)
    {
        int messageCount;
        lock (_lock)
        {
            if (!_messages.TryGetValue(chatId, out var messages))
            {
                messages = new List<long>();
                _messages[chatId] = messages;
            }
            
            messages.Add(messageId);
            messageCount = messages.Count;
        }
        
        if (UseLogging)
            Debug.Log($"Message {messageId} add from chat {chatId}. message count: {messageCount}", "TemporaryMessageLimiterModule");
        
        await Task.CompletedTask;
    }

    public async Task UnregisterMessage(long chatId, long messageId)
    {
        int messageCount;
        lock (_lock)
        {
            if (!_messages.TryGetValue(chatId, out var messages))
                return;
            
            messages.Remove(messageId);
            
            messageCount = messages.Count;
            if (messageCount == 0)
                _messages.Remove(chatId);
        }
        
        if (UseLogging)
            Debug.Log($"Message {messageId} removed from chat {chatId}. Remaining messages: {messageCount}", "TemporaryMessageLimiterModule");
        
        await Task.CompletedTask;
    }
}