using TgCore.Api.Requests.Parameters;
using TgCore.Api.Types.File;

namespace TgCore.Api.Requests;

public partial class TelegramRequests
{
    public async Task<RequestResponse<Message>> EditText(
        long chatId, 
        long messageId, 
        string text,
        IKeyboardMarkup? keyboard = null, 
        ParseMode? parseMode = null, 
        ShortParameters? shortParameters = null, 
        CancellationToken ct = default)
    {
        var pm = parseMode ?? _bot.Options.DefaultParseMode;
        var parameters = new TelegramParametersBuilder()
            .Add("chat_id", chatId)
            .Add("message_id", messageId)
            .Add("text", TextFormatter?.Process(text, pm) ?? text)
            .Add("parse_mode", BotHelper.GetParseModeName(pm))
            .Add("reply_markup", keyboard)
            .AddDictionary(shortParameters?.ToDictionary())
            .Build();
        
        return await SendRequest<Message>(TelegramMethods.EDIT_MESSAGE_TEXT, parameters, ct:ct);
    }
    
    public async  Task<RequestResponse<Message>> EditMedia(
        long chatId, 
        long messageId, 
        InputFile file, 
        string? caption = null,
        IKeyboardMarkup? keyboard = null,
        ParseMode? parseMode = null,
        ShortParameters? shortParameters = null, 
        CancellationToken ct = default)
    {
        var pm = parseMode ?? _bot.Options.DefaultParseMode;
        var media = new
        {
            type = BotHelper.GetMediaType(file.FileType),
            media = file.GetValue(),
            caption = string.IsNullOrEmpty(caption) ? null : TextFormatter?.Process(caption, pm) ?? caption,
            parse_mode = caption != null 
                ? BotHelper.GetParseModeName(parseMode ?? _bot.Options.DefaultParseMode) 
                : null
        };
            
        var parameters = new TelegramParametersBuilder()
            .Add("chat_id", chatId)
            .Add("message_id", messageId)
            .Add("media", media)
            .Add("reply_markup", keyboard)
            .AddDictionary(shortParameters?.ToDictionary())
            .Build();

        return await SendRequest<Message>(TelegramMethods.EDIT_MESSAGE_MEDIA, parameters,ct:ct);
    }

    public async Task<RequestResponse<Message>> EditKeyboard(
        long chatId,
        long messageId,
        IKeyboardMarkup? keyboard = null,
        ShortParameters? shortParameters = null,
        CancellationToken ct = default)
    {
        var parameters = new TelegramParametersBuilder()
            .Add("chat_id", chatId)
            .Add("message_id", messageId)
            .Add("reply_markup", keyboard)
            .AddDictionary(shortParameters?.ToDictionary())
            .Build();

        return await SendRequest<Message>(TelegramMethods.EDIT_MESSAGE_REPLY_MARKUP, parameters, ct);
    }

    public async Task<RequestResponse<Message>> EditCaption(
        long chatId,
        long messageId,
        string caption,
        bool showCaptionAboveMedia = false,
        IKeyboardMarkup? keyboard = null,
        ParseMode? parseMode = null,
        ShortParameters? shortParameters = null,
        CancellationToken ct = default)
    {
        var pm = parseMode ?? _bot.Options.DefaultParseMode;
        var parameters = new TelegramParametersBuilder()
            .Add("chat_id", chatId)
            .Add("message_id", messageId)
            .Add("caption", TextFormatter?.Process(caption, pm) ?? caption)
            .Add("parse_mode", BotHelper.GetParseModeName(parseMode ?? _bot.Options.DefaultParseMode))
            .Add("reply_markup", keyboard)
            .Add("show_caption_above_media", showCaptionAboveMedia)
            .AddDictionary(shortParameters?.ToDictionary())
            .Build();

        return await SendRequest<Message>(TelegramMethods.EDIT_MESSAGE_CAPTION, parameters, ct);
    }
}