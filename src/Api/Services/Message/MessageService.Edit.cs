namespace TgCore.Api.Services.Message;

public partial class MessageService
{
    public async Task<Types.Message?> EditText(long chatId, long messageId, string text,
        IKeyboardMarkup? keyboard = null, ParseMode? parseMode = null)
    {
        try
        {
            await ApplyRateLimit();

            var message = await _bot.Client.CallAsync<Types.Message>(TelegramMethods.EDIT_MESSAGE_TEXT, new
            {
                chat_id = chatId,
                message_id = messageId,
                text = text,
                parse_mode = BotHelper.GetParseModeName(parseMode ?? _bot.Options.DefaultParseMode),
                reply_markup = keyboard
            });

            return message;
        }
        catch (Exception ex)
        {
            await _bot.AddException(ex);
            return null;
        }
    }
    
    public async Task<Types.Message?> EditMedia(long chatId, long messageId, InputFile file, string? caption = null,
        IKeyboardMarkup? keyboard = null,
        ParseMode? parseMode = null)
    {
        try
        {
            await ApplyRateLimit();
            
            var mediaType = file.FileType switch
            {
                InputFileType.Photo => "photo",
                InputFileType.Video => "video",
                InputFileType.Document => "document",
                InputFileType.Audio => "audio",
                InputFileType.Animation => "animation",
                _ => throw new NotSupportedException($"File type {file.FileType} is not supported for editing")
            };
            
            var media = new
            {
                type = mediaType,
                media = file.GetValue(),
                caption = caption,
                parse_mode = caption != null 
                    ? BotHelper.GetParseModeName(parseMode ?? _bot.Options.DefaultParseMode) 
                    : null
            };

            var message = await _bot.Client.CallAsync<Types.Message?>(TelegramMethods.EDIT_MESSAGE_MEDIA, new
            {
                chat_id = chatId,
                message_id = messageId,
                media = media,
                reply_markup = keyboard
            });

            return message;
        }
        catch (Exception ex)
        {
            await _bot.AddException(ex);
            return null;
        }
    }

    public async Task<Types.Message?> EditKeyboard(
        long chatId,
        long messageId,
        IKeyboardMarkup keyboard)
    {
        try
        {
            await ApplyRateLimit();

            return await _bot.Client.CallAsync<Types.Message>(TelegramMethods.EDIT_MESSAGE_REPLY_MARKUP, new
            {
                chat_id = chatId,
                message_id = messageId,
                reply_markup = keyboard
            });
        }
        catch (Exception ex)
        {
            await _bot.AddException(ex);
            return null;
        }
    }

    public async Task<Types.Message?> EditCaption(
        long chatId,
        long messageId,
        string caption,
        IKeyboardMarkup? keyboard = null,
        ParseMode? parseMode = null)
    {
        try
        {
            await ApplyRateLimit();

            return await _bot.Client.CallAsync<Types.Message>(TelegramMethods.EDIT_MESSAGE_CAPTION, new
            {
                chat_id = chatId,
                message_id = messageId,
                caption = caption,
                parse_mode = BotHelper.GetParseModeName(parseMode ?? _bot.Options.DefaultParseMode),
                reply_markup = keyboard
            });
        }
        catch (Exception ex)
        {
            await _bot.AddException(ex);
            return null;
        }
    }
}