namespace TgCore.Sdk.Services;

public class BotTemporaryMessageService
{
    private readonly Dictionary<long, List<(long, DateTime)>> _messages = new();
    private readonly object _lock = new();

    private readonly TelegramBot _bot;

    private Func<long, long, Task>? _onAdd;
    private Func<long, long, Task>? _onDelete;
    
    public int MaxMessagesPerUser { get; set; } = 5;
    public bool UseMaxMessages { get; set; } = true;

    public BotTemporaryMessageService(
        TelegramBot bot,
        BotTaskLoop loop,
        Func<long, long, Task>? onAdd = null,
        Func<long, long, Task>? onDelete = null)
    {
        _bot = bot;

        _onAdd = onAdd;
        _onDelete = onDelete;

        loop.AddRepeatingTask(TimeSpan.FromSeconds(1), SafeCheckForDelete, DateTime.Now.AddSeconds(5));
    }

    public bool SetTemporary(long fromId, long messageId, TimeSpan? lifetime = null)
    {
        lock (_lock)
        {
            if (UseMaxMessages && GetMessageCount(fromId) >= MaxMessagesPerUser) return false;

            lifetime ??= TimeSpan.FromSeconds(5);

            var expiresAt = DateTime.UtcNow.Add(lifetime.Value);

            if (!_messages.ContainsKey(fromId))
                _messages[fromId] = new List<(long, DateTime)>();

            _messages[fromId].Add((messageId, expiresAt));
        }

        if (_onAdd != null)
            _onAdd.Invoke(fromId, messageId);
        
        return true;
    }

    public async Task<bool> SendTemporaryText(long fromId, string text, IKeyboardMarkup? keyboard = null,
        long? replyId = null, TimeSpan? lifetime = null)
    {
        if (UseMaxMessages)
        {
            lock (_lock)
            {
                if (GetMessageCount(fromId) >= MaxMessagesPerUser)
                    return false;
            }
        }
        
        var message = await _bot.Requests.SendText(fromId, text, keyboard: keyboard, replyId: replyId);
        if (message == null) return false;

        return SetTemporary(fromId, message.Id, lifetime);
    }

    public bool RemoveMessage(long userId, long messageId)
    {
        lock (_lock)
        {
            if (!_messages.TryGetValue(userId, out var userMessages))
                return false;

            var removedCount = userMessages.RemoveAll(x => x.Item1 == messageId);
            if (removedCount == 0) return false;

            if (userMessages.Count == 0)
                _messages.Remove(userId);

            return true;
        }
    }

    protected void AddOnDelete(Func<long, long, Task> onDelete) => _onDelete = onDelete;
    protected void AddOnAdd(Func<long, long, Task> onAdd) => _onAdd = onAdd;

    public void ClearUserMessages(long userId)
    {
        lock (_lock)
        {
            _messages.Remove(userId);
        }
    }

    public int GetMessageCount(long userId)
    {
        lock (_lock)
        {
            return _messages.TryGetValue(userId, out var messages) ? messages.Count : 0;
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
        var actuallyDeleted = new List<(long userId, long messageId)>();

        lock (_lock)
        {
            foreach (var message in toDelete)
            {
                if (RemoveMessage(message.Item1, message.Item2))
                {
                    actuallyDeleted.Add(message);
                }
            }
        }

        foreach (var message in actuallyDeleted)
        {
            await _bot.Requests.DeleteMessage(message.Item1, message.Item2);

            if (_onDelete != null)
                await _onDelete.Invoke(message.Item1, message.Item2);
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
            await _bot.AddException(ex);
        }
    }
}