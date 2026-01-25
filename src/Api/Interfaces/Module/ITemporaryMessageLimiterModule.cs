namespace TgCore.Api.Interfaces.Module;

public interface ITemporaryMessageLimiterModule
{
    int MaxMessageLimit { get; set; }

    Task<bool> CanSend(long chatId);
    Task RegisterMessage(long chatId, long messageId);
    Task UnregisterMessage(long chatId, long messageId);
}