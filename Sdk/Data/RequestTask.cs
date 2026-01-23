namespace TgCore.Sdk.Data;

public class RequestTask
{
    public Func<Task> Execute { get; }
    public int Priority { get; }
    public int MaxRetries { get; }
    public DateTime CreatedAt { get; }
    public int RetryCount { get; set; }

    public RequestTask(Func<Task> execute, int priority = 1, int maxRetries = 5)
    {
        Execute = execute;
        Priority = priority;
        MaxRetries = maxRetries;
        CreatedAt = DateTime.UtcNow;
    }
}