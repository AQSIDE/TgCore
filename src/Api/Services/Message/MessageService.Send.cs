namespace TgCore.Api.Services.Message;

public partial class MessageService
{
    public async Task<Types.Message?> SendText(long chatId, string text, IKeyboardMarkup? keyboard = null,
        long? replyId = null,
        ParseMode? parseMode = null, TimeSpan? lifeTime = null)
    {
        try
        {
            if (!await CanSendTemporary(chatId, lifeTime)) return null;

            await ApplyRateLimit();

            var message = await _bot.Client.CallAsync<Types.Message?>(TelegramMethods.SEND_MESSAGE, new
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

    public async Task<Types.Message?> SendMedia(long chatId, InputFile file, string? caption = null,
        IKeyboardMarkup? keyboard = null,
        long? replyId = null,
        ParseMode? parseMode = null, TimeSpan? lifeTime = null)
    {
        try
        {
            if (!await CanSendTemporary(chatId, lifeTime)) return null;

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

            var message = await _bot.Client.CallAsync<Types.Message?>(method, new
            {
                chat_id = chatId,
                caption = caption,
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
}