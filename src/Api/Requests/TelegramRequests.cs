using TgCore.Diagnostics.Debugger;

namespace TgCore.Api.Requests;

public partial class TelegramRequests
{
    private readonly TelegramBot _bot;

    private ILifetimeModule? Lifetime => _bot.Options.Lifetime;
    private IRateLimitModule? RateLimit => _bot.Options.RateLimit;
    private ITextFormatterModule? TextFormatter => _bot.Options.TextFormatter;
    private ITemporaryMessageLimiterModule? TemporaryMessageLimiter => _bot.Options.TemporaryMessageLimiter;

    internal TelegramRequests(TelegramBot bot)
    {
        _bot = bot;

        if (Lifetime != null)
            Lifetime.OnDelete = UnregisterTemporaryMessage;
    }
    
    public async Task<(bool Ok, T? Result)> SendRequest<T>(string method, object? body = null,
        JsonSerializerOptions? options = null)
    {
        try
        {
            await ApplyRateLimit();

            return (true, await _bot.Client.CallAsync<T>(method, body, options));
        }
        catch (Exception ex)
        {
            await _bot.AddException(ex);
            return (false, default);
        }
    }

    private async Task ApplyLifetime(Message? message, long chatId, TimeSpan? lifeTime)
    {
        if (message == null) return;
        
        if (lifeTime != null && Lifetime == null)
        {
            Debug.Console.LogWarning(
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
            Debug.Console.LogWarning("ITemporaryMessageLimiterModule requires ILifetimeModule to work. " +
                                     "Temporary message limiter will be ignored. " +
                                     "Set Lifetime module in bot options.");
            return true;
        }

        if (TemporaryMessageLimiter == null || Lifetime == null)
            return true;

        return await TemporaryMessageLimiter.CanSend(chatId);
    }
}