namespace TgCore.Api.Helpers;

public static class BotHelper
{
    public static string GetPollType(PollType type)
    {
        return type switch
        {
            PollType.Regular => "regular",
            PollType.Quiz => "quiz",
            _ => throw new NotImplementedException("unsupported poll type"),
        };
    }
    
    public static string GetMediaType(InputFileType fileType)
    {
        return fileType switch
        {
            InputFileType.Photo => "photo",
            InputFileType.Video => "video",
            InputFileType.Document => "document",
            InputFileType.Audio => "audio",
            InputFileType.Animation => "animation",
            _ => throw new NotImplementedException("unsupported file Type"),
        };
    }
    
    public static string[] GetAllowedUpdatesNames(UpdateType[] allowedUpdates)
    {
        string GetName(UpdateType type)
        {
            return type switch
            {
                UpdateType.Message => "message",
                UpdateType.CallbackQuery => "callback_query",
                UpdateType.EditedMessage => "edited_message",
                UpdateType.ChannelPost => "channel_post",
                UpdateType.EditedChannelPost => "edited_channel_post",
                UpdateType.InlineQuery => "inline_query",
                UpdateType.BusinessConnection => "business_connection",
                UpdateType.BusinessMessage => "business_message",
                UpdateType.EditedBusinessMessage => "edited_business_message",
                UpdateType.DeletedBusinessMessages => "deleted_business_messages",
                UpdateType.MessageReaction => "message_reaction",
                UpdateType.MessageReactionCount => "message_reaction_count",
                UpdateType.ChatBoost => "chat_boost",
                UpdateType.ChatBoostRemoved => "chat_boost_removed",
                UpdateType.Poll => "poll",
                UpdateType.PollAnswer => "poll_answer",
                UpdateType.PreCheckoutQuery => "precheck_out_query",
                UpdateType.ShippingQuery => "shipping_query",
                UpdateType.PurchasedPaidMedia => "purchased_paid_media",
                UpdateType.ChatMember => "chat_member",
                UpdateType.MyChatMember => "my_chat_member",
                UpdateType.ChatJoinRequest => "chat_join_request",
                _ => "unknown",
            };
        }
        
        return allowedUpdates.Select(GetName).ToArray();
    }

    public static string GetParseModeName(ParseMode mode)
    {
        return mode switch
        {
            ParseMode.HTML => "HTML",
            ParseMode.Markdown => "Markdown",
            ParseMode.MarkdownV2 => "MarkdownV2",
            _ => "none"
        };
    }
    
    public static void SetUpdateType(Update update)
    {
        update.Type = update switch
        {
            { Message: not null } => UpdateType.Message,
            { EditedMessage: not null } => UpdateType.EditedMessage,
            { CallbackQuery: not null } => UpdateType.CallbackQuery,
            { ChannelPost: not null } => UpdateType.ChannelPost,
            { EditedChannelPost: not null } => UpdateType.EditedChannelPost,
            { InlineQuery: not null } => UpdateType.InlineQuery,
            { ChosenInlineResult: not null } => UpdateType.ChosenInlineResult,
            { BusinessConnection: not null } => UpdateType.BusinessConnection,
            { BusinessMessage: not null } => UpdateType.BusinessMessage,
            { EditedBusinessMessage: not null } => UpdateType.EditedBusinessMessage,
            { DeletedBusinessMessages: not null } => UpdateType.DeletedBusinessMessages,
            // { MessageReaction: not null } => UpdateType.MessageReaction,
            // { MessageReactionCount: not null } => UpdateType.MessageReactionCount,
            // { ChatBoost: not null } => UpdateType.ChatBoost,
            // { ChatBoostRemoved: not null } => UpdateType.ChatBoostRemoved,
            { Poll: not null } => UpdateType.Poll,
            { PollAnswer: not null } => UpdateType.PollAnswer,
            // { ShippingQuery: not null } => UpdateType.ShippingQuery,
            // { PreCheckoutQuery: not null } => UpdateType.PreCheckoutQuery,
            // { PurchasedPaidMedia: not null } => UpdateType.PurchasedPaidMedia,
            // { ChatMember: not null } => UpdateType.ChatMember,
            // { MyChatMember: not null } => UpdateType.MyChatMember,
            // { ChatJoinRequest: not null } => UpdateType.ChatJoinRequest,
            _ => UpdateType.Unknown
        };
    }
}