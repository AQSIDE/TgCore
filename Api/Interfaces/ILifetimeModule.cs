namespace TgCore.Api.Interfaces;

public interface ILifetimeModule
{
    Func<long, long, Task>? OnAdd { get; set; }
    Func<long, long, Task>? OnDelete { get; set; }
    public Task Set(long chatId, long messageId, TimeSpan lifetime);
    public Task<bool> Remove(long chatId, long messageId);
    public void ClearMessages(long chatId);
}