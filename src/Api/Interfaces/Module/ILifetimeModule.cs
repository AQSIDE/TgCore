namespace TgCore.Api.Interfaces.Module;

public interface ILifetimeModule
{
    /// <summary>
    /// long - chatId/userId, long - messageId
    /// </summary>
    Func<long, long, Task>? OnAdd { get; set; }
    
    /// <summary>
    /// long - chatId/userId, long - messageId
    /// </summary>
    Func<long, long, Task>? OnDelete { get; set; }
    public Task Set(long chatId, long messageId, TimeSpan lifetime);
    public Task<bool> Remove(long chatId, long messageId);
    public Task<bool> Delete(long chatId, long messageId);
    public void ClearMessages(long chatId);
}