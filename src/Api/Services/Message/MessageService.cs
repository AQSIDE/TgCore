using TgCore.Diagnostics.Debugger;

namespace TgCore.Api.Services.Message;

public partial class MessageService
{
    private readonly TelegramBot _bot;

    private ILifetimeModule? Lifetime => _bot.Options.Lifetime;
    private IRateLimitModule? RateLimit => _bot.Options.RateLimit;
    private ITemporaryMessageLimiterModule? TemporaryMessageLimiter => _bot.Options.TemporaryMessageLimiter;

    internal MessageService(TelegramBot bot)
    {
        _bot = bot;

        if (Lifetime != null)
            Lifetime.OnDelete = UnregisterTemporaryMessage;
    }
    

    private async Task ApplyLifetime(Types.Message? message, long chatId, TimeSpan? lifeTime)
    {
        if (message == null) return;
        
        if (lifeTime != null && Lifetime == null)
        {
            Debug.LogWarning(
                "Message lifetime is set, but ILifetimeModule is not configured. " +
                $"Message {message.Id} in chat {chatId} will not be automatically deleted. " +
                "Set Lifetime module in bot options."
            );
            return;
        }
        
        if (lifeTime == null || Lifetime == null)
            return;

        await Lifetime.Set(chatId, message.Id, lifeTime.Value);

        if (TemporaryMessageLimiter != null)
        {
            await TemporaryMessageLimiter.RegisterMessage(chatId, message.Id);
        }
    }

    private async Task ApplyRateLimit()
    {
        if (RateLimit != null)
        {
            await RateLimit.WaitAsync();
        }
    }

    private async Task UnregisterLifetime(long chatId, long messageId)
    {
        if (Lifetime != null)
        {
            await Lifetime.Remove(chatId, messageId);

            await UnregisterTemporaryMessage(chatId, messageId);
        }
    }

    private async Task UnregisterTemporaryMessage(long chatId, long messageId)
    {
        if (TemporaryMessageLimiter != null && Lifetime != null)
        {
            await TemporaryMessageLimiter.UnregisterMessage(chatId, messageId);
        }
    }

    private async Task<bool> CanSendTemporary(long chatId, TimeSpan? lifeTime)
    {
        if (lifeTime == null) return true;

        if (Lifetime == null && TemporaryMessageLimiter != null)
        {
            Debug.LogWarning("ITemporaryMessageLimiterModule requires ILifetimeModule to work. " +
                             "Temporary message limiter will be ignored. " +
                             "Set Lifetime module in bot options.");
            return true;
        }

        if (TemporaryMessageLimiter == null || Lifetime == null)
            return true;

        return await TemporaryMessageLimiter.CanSend(chatId);
    }
}