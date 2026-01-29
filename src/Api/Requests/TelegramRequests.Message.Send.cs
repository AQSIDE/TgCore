using TgCore.Api.Requests.Parameters;
using TgCore.Api.Types.File;
using TgCore.Diagnostics.Debugger;

namespace TgCore.Api.Requests;

public partial class TelegramRequests
{
    public async Task<Message?> SendText(long chatId, string text, IKeyboardMarkup? keyboard = null,
        long? replyId = null,
        ParseMode? parseMode = null, 
        TimeSpan? lifeTime = null, 
        DefaultParameters? defaultParameters = null)
    {
        try
        {
            if (!await CanSendTemporary(chatId, lifeTime)) return null;

            await ApplyRateLimit();

            var pm = parseMode ?? _bot.Options.DefaultParseMode;
            var parameters = new TelegramParametersBuilder()
                .Add("chat_id", chatId)
                .Add("text", TextFormatter?.Process(text, pm) ?? text)
                .Add("parse_mode", BotHelper.GetParseModeName(pm))
                .Add("reply_to_message_id", replyId)
                .Add("allow_sending_without_reply", true)
                .Add("reply_markup", keyboard)
                .AddDictionary(defaultParameters?.ToDictionary())
                .Build();

            var message = await _bot.Client.CallAsync<Message?>(TelegramMethods.SEND_MESSAGE, parameters);

            await ApplyLifetime(message, chatId, lifeTime);

            return message;
        }
        catch (Exception ex)
        {
            await _bot.AddException(ex);
            return null;
        }
    }

    public async Task<Message?> SendMedia(long chatId, InputFile file, string? caption = null,
        IKeyboardMarkup? keyboard = null,
        long? replyId = null,
        ParseMode? parseMode = null, 
        TimeSpan? lifeTime = null, 
        DefaultParameters? defaultParameters = null)
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
            
            var pm = parseMode ?? _bot.Options.DefaultParseMode;
            var parameters = new TelegramParametersBuilder()
                .Add("chat_id", chatId)
                .Add("caption", string.IsNullOrEmpty(caption) ? null : TextFormatter?.Process(caption, pm) ?? caption)
                .Add("photo", file.FileType == InputFileType.Photo ? file.GetValue() : null)
                .Add("video", file.FileType == InputFileType.Video ? file.GetValue() : null)
                .Add("document", file.FileType == InputFileType.Document ? file.GetValue() : null)
                .Add("audio", file.FileType == InputFileType.Audio ? file.GetValue() : null)
                .Add("animation", file.FileType == InputFileType.Animation ? file.GetValue() : null)
                .Add("parse_mode", BotHelper.GetParseModeName(pm))
                .Add("has_spoiler", file.HasSpoiler)
                .Add("reply_to_message_id", replyId)
                .Add("allow_sending_without_reply", true)
                .Add("show_caption_above_media", file.ShowCaptionAboveMedia)
                .Add("reply_markup", keyboard)
                .AddDictionary(defaultParameters?.ToDictionary())
                .Build();

            var message = await _bot.Client.CallAsync<Message?>(method, parameters);

            await ApplyLifetime(message, chatId, lifeTime);

            return message;
        }
        catch (Exception ex)
        {
            await _bot.AddException(ex);
            return null;
        }
    }

    public async Task<Message[]?> SendMediaGroup(
        long chatId, 
        InputFile[] files, 
        string? caption = null,
        IKeyboardMarkup? keyboard = null,
        long? replyId = null,
        ParseMode? parseMode = null, 
        TimeSpan? lifeTime = null,
        DefaultParameters? defaultParameters = null)
    {
        try
        {
            if (!await CanSendTemporary(chatId, lifeTime)) return null;

            await ApplyRateLimit();

            var pm = parseMode ?? _bot.Options.DefaultParseMode;
            var media = files.Select(file => new
            {
                type = BotHelper.GetMediaType(file.FileType),
                media = file.GetValue(),
                caption =  string.IsNullOrEmpty(caption) ? null : TextFormatter?.Process(caption, pm) ?? caption,
                parse_mode = BotHelper.GetParseModeName(pm),
                has_spoiler = file.HasSpoiler
            }).ToArray();
            
            var parameters = new TelegramParametersBuilder()
                .Add("chat_id", chatId)
                .Add("media", media)
                .Add("reply_to_message_id", replyId)
                .Add("allow_sending_without_reply", true)
                .Add("show_caption_above_media", files.FirstOrDefault()?.ShowCaptionAboveMedia ?? false)
                .Add("reply_markup", keyboard)
                .AddDictionary(defaultParameters?.ToDictionary())
                .Build();

            var message = await _bot.Client.CallAsync<Message[]?>(TelegramMethods.SEND_MEDIA_GROUP, parameters);

            if (message != null)
            {
                foreach (var msg in message)
                {
                    await ApplyLifetime(msg, chatId, lifeTime);
                }
            }

            return message;
        }
        catch (Exception ex)
        {
            await _bot.AddException(ex);
            return null;
        }
    }
}