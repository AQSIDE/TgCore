namespace TgCore.Api.Services;

public class MessageService
{
    private readonly TelegramBot _bot;

    private ILifetimeModule? Lifetime => _bot.Options.LtModule;
    private IRateLimitModule? RateLimit => _bot.Options.RlModule;

    internal MessageService(TelegramBot bot)
    {
        _bot = bot;
    }
    
    public async Task<Message?> SendText(long chatId, string text, IKeyboardMarkup? keyboard = null,
        long? replyId = null,
        ParseMode? parseMode = null, TimeSpan? lifeTime = null)
    {
        try
        {
            await ApplyRateLimit();
            
            var message = await _bot.Client.CallAsync<Message?>(TelegramMethods.SEND_MESSAGE, new
            {
                chat_id = chatId,
                text = text,
                parse_mode = BotHelper.GetParseModeName(parseMode ?? _bot.Options.DefaultParseMode),
                reply_to_message_id = replyId,
                allow_sending_without_reply = true,
                reply_markup = keyboard
            });

            await ApplyLifetime(message, chatId, lifeTime);

            return message;
        }
        catch (Exception ex)
        {
            await _bot.AddException(ex, null);
            return null;
        }
    }
    
    public async Task<Message?> SendMedia(long chatId, InputFile file, string? text = null, IKeyboardMarkup? keyboard = null,
        long? replyId = null,
        ParseMode? parseMode = null, TimeSpan? lifeTime = null)
    {
        try
        {
            await ApplyRateLimit();
            
            var method = file.FileType switch
            {
                InputFileType.Photo => TelegramMethods.SEND_PHOTO,
                InputFileType.Video => TelegramMethods.SEND_VIDEO,
                InputFileType.Document => TelegramMethods.SEND_DOCUMENT,
                InputFileType.Audio => TelegramMethods.SEND_AUDIO,
                InputFileType.Animation => TelegramMethods.SEND_ANIMATION,
                _ => throw new NotSupportedException()
            };
            
            var message = await _bot.Client.CallAsync<Message?>(method, new
            {
                chat_id = chatId,
                caption = text,
                photo = file.FileType == InputFileType.Photo ? file.GetValue() : null,
                video = file.FileType == InputFileType.Video ? file.GetValue() : null,
                document = file.FileType == InputFileType.Document ? file.GetValue() : null,
                audio = file.FileType == InputFileType.Audio ? file.GetValue() : null,
                animation = file.FileType == InputFileType.Animation ? file.GetValue() : null,
                parse_mode = BotHelper.GetParseModeName(parseMode ?? _bot.Options.DefaultParseMode),
                reply_to_message_id = replyId,
                allow_sending_without_reply = true,
                reply_markup = keyboard
            });

            await ApplyLifetime(message, chatId, lifeTime);

            return message;
        }
        catch (Exception ex)
        {
            await _bot.AddException(ex, null);
            return null;
        }
    }

    public async Task<Message?> EditText(long chatId, long messageId, string text,
        IKeyboardMarkup? keyboard = null, long? replyId = null, ParseMode? parseMode = null)
    {
        try
        {
            await ApplyRateLimit();
            
            var message = await _bot.Client.CallAsync<Message>(TelegramMethods.EDIT_MESSAGE_TEXT, new
            {
                chat_id = chatId,
                message_id = messageId,
                text = text,
                parse_mode = BotHelper.GetParseModeName(parseMode ?? _bot.Options.DefaultParseMode),
                reply_to_message_id = replyId,
                allow_sending_without_reply = true,
                reply_markup = keyboard
            });

            return message;
        }
        catch (Exception ex)
        {
            await _bot.AddException(ex, null);
            return null;
        }
    }

    public async Task<bool> DeleteMessage(long chatId, long messageId)
    {
        try
        {
            await ApplyRateLimit();
            await UnregisterLifetime(chatId, messageId);
            
            return await _bot.Client.CallAsync<bool>(TelegramMethods.DELETE_MESSAGE, new
            {
                chat_id = chatId,
                message_id = messageId
            });
        }
        catch (Exception ex)
        {
            await _bot.AddException(ex, null);
            return false;
        }
    }

    public async Task<bool> AnswerCallback(string callbackId, string? text = null, bool showAlert = false)
    {
        try
        {
            await ApplyRateLimit();
            
            return await _bot.Client.CallAsync<bool>(TelegramMethods.ANSWER_CALLBACK_QUERY, new
            {
                callback_query_id = callbackId,
                text = text,
                show_alert = showAlert
            });
        }
        catch (Exception ex)
        {
            await _bot.AddException(ex, null);
            return false;
        }
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
            await _bot.AddException(ex, null);
            return (false, default);
        }
    }

    private async Task ApplyLifetime(Message? message, long chatId, TimeSpan? lifeTime)
    {
        if (message != null && lifeTime != null)
        {
            if (Lifetime != null)
            {
                await Lifetime.Set(chatId, message.Id, lifeTime.Value);
            }
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
        }
    }
}