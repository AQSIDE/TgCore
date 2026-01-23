using TgCore.Api.Looping;

namespace TgCore.Api.Modules;

public class LifetimeModule : ILifetimeModule
{
    private readonly Dictionary<long, List<(long, DateTime)>> _messages = new();
    private readonly object _lock = new();

    private readonly TelegramBot _bot;

    public Func<long, long, Task>? OnAdd { get; set; }
    public Func<long, long, Task>? OnDelete { get; set; }

    public LifetimeModule(
        TelegramBot bot,
        TaskLoop loop,
        Func<long, long, Task>? onAdd = null,
        Func<long, long, Task>? onDelete = null)
    {
        _bot = bot;

        OnAdd = onAdd;
        OnDelete = onDelete;

        loop.AddRepeatingTask(TimeSpan.FromSeconds(1), SafeCheckForDelete, DateTime.Now.AddSeconds(5));
    }
    
    public async Task Set(long chatId, long messageId, TimeSpan lifetime)
    {
        lock (_lock)
        {
            var expiresAt = DateTime.UtcNow.Add(lifetime);

            if (!_messages.ContainsKey(chatId))
                _messages[chatId] = new List<(long, DateTime)>();

            _messages[chatId].Add((messageId, expiresAt));
        }

        if (OnAdd != null)
            await OnAdd.Invoke(chatId, messageId);
    }

    public async Task<bool> Remove(long chatId, long messageId)
    {
        lock (_lock)
        {
            if (!_messages.TryGetValue(chatId, out var userMessages))
                return false;

            var removedCount = userMessages.RemoveAll(x => x.Item1 == messageId);
            if (removedCount == 0) return false;

            if (userMessages.Count == 0)
                _messages.Remove(chatId);
        }
        
        if (OnDelete != null)
            await OnDelete.Invoke(chatId, messageId);
        
        return true;
    }

    public void ClearMessages(long chatId)
    {
        lock (_lock)
        {
            _messages.Remove(chatId);
        }
    }

    public int GetMessageCount(long chatId)
    {
        lock (_lock)
        {
            return _messages.TryGetValue(chatId, out var messages) ? messages.Count : 0;
        }
    }

    public int GetTotalMessageCount()
    {
        lock (_lock)
        {
            return _messages.Values.Sum(list => list?.Count ?? 0);
        }
    }

    private async Task CheckForDelete()
    {
        var messagesToDelete = new List<(long, long)>();

        lock (_lock)
        {
            if (_messages.Count == 0) return;

            var now = DateTime.UtcNow;

            foreach (var kvp in _messages)
            {
                var userId = kvp.Key;
                var messages = kvp.Value;

                foreach (var message in messages)
                {
                    var messageId = message.Item1;
                    var expiresAt = message.Item2;

                    if (expiresAt < now)
                        messagesToDelete.Add((userId, messageId));
                }
            }
        }

        if (messagesToDelete.Count > 0)
            await DeleteMessages(messagesToDelete);
    }

    private async Task DeleteMessages(List<(long, long)> toDelete)
    {
        foreach (var message in toDelete)
        {
            await _bot.Message.DeleteMessage(message.Item1, message.Item2);
            await Remove(message.Item1, message.Item2);
        }
    }

    private async Task SafeCheckForDelete()
    {
        try
        {
            await CheckForDelete();
        }
        catch (Exception ex)
        {
            await _bot.AddException(ex, null);
        }
    }
}