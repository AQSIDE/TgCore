namespace TgCore.Sdk.Looping.Types;

internal class ScheduledTask
{
    public DateTime ExecuteAt { get; private set; }
    public Func<Task> Action { get; }
    public TimeSpan? RepeatInterval { get; }
    
    public bool IsRepeating => RepeatInterval.HasValue;

    public ScheduledTask(Func<Task> action, DateTime executeAt, TimeSpan? repeatInterval = null)
    {
        Action = action;
        ExecuteAt = executeAt;
        RepeatInterval = repeatInterval;
    }

    public async Task ExecuteAsync()
    {
        await Action();
        if (RepeatInterval.HasValue)
        {
            ExecuteAt = ExecuteAt.Add(RepeatInterval.Value);
        }
    }
}