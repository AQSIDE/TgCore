namespace TgCore.Sdk.Looping.Types;

public class BotTaskLoop : BotLoop
{
    private readonly List<ScheduledTask> _tasks = new();
    private readonly TelegramBot? _bot;

    public BotTaskLoop(int intervalMs = 100, TelegramBot? bot = null)
    {
        IntervalMs = intervalMs;
        _bot = bot;
    }
    
    public void AddTask(DateTime executeAt, Func<Task> action)
    {
        _tasks.Add(new ScheduledTask(action, executeAt));
    }
    
    public void AddRepeatingTask(TimeSpan interval, Func<Task> action, DateTime? startAt = null)
    {
        var executeAt = startAt ?? DateTime.Now.Add(interval);
        _tasks.Add(new ScheduledTask(action, executeAt, interval));
    }

    public override async Task OnTick()
    {
        if (_tasks.Count == 0) return;
        
        var toExecute = _tasks.Where(t => t.ExecuteAt <= DateTime.Now).ToList();
        if (!toExecute.Any()) return;
        
        foreach (var task in toExecute)
        {
            try
            {
                await task.ExecuteAsync();
            }
            catch (Exception ex)
            {
                if (_bot != null)
                    await _bot.AddException(ex, null);
            }
            finally
            {
                if (!task.IsRepeating)
                    _tasks.Remove(task);
            }
        }
    }
}