namespace TgCore.Api.Interfaces.Runtime;

public interface IUpdateReceiver
{
    Task StartReceiving(
        IReadOnlyList<Func<Update, Task>> updateHandlers,
        IReadOnlyList<Func<Exception, Task>> errorHandlers,
        CancellationToken ct);
}