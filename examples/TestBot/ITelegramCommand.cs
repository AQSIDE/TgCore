namespace TestBot;

public interface ITelegramCommand
{
    string Name { get; }
    Task ExecuteAsync(long chatId, long? messageId, string[]? args = null);
}