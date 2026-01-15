namespace TgCore.Sdk.Looping.Types;

public class BotTaskLoop : BotLoop
{
    private readonly List<ScheduledTask> _tasks = new();
    private readonly Func<Exception, Task>? _onError;

    public BotTaskLoop(int intervalMs = 100, Func<Exception, Task>? onError = null)
    {
        IntervalMs = intervalMs;
        _onError = onError;
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

    protected override async Task OnTick()
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
                if (_onError != null)
                    await _onError.Invoke(ex);
            }
            finally
            {
                if (!task.IsRepeating)
                    _tasks.Remove(task);
            }
        }
    }
}