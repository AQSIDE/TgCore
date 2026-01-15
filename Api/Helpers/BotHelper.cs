using TgCore.Api.Enums;
using TgCore.Api.Types;

namespace TgCore.Api.Helpers;

internal class BotHelper
{
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
            _ => throw new ArgumentOutOfRangeException(nameof(mode), "Unsupported parse mode")
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
            // { BusinessConnection: not null } => UpdateType.BusinessConnection,
            // { BusinessMessage: not null } => UpdateType.BusinessMessage,
            // { EditedBusinessMessage: not null } => UpdateType.EditedBusinessMessage,
            // { DeletedBusinessMessages: not null } => UpdateType.DeletedBusinessMessages,
            // { MessageReaction: not null } => UpdateType.MessageReaction,
            // { MessageReactionCount: not null } => UpdateType.MessageReactionCount,
            // { ChatBoost: not null } => UpdateType.ChatBoost,
            // { ChatBoostRemoved: not null } => UpdateType.ChatBoostRemoved,
            // { Poll: not null } => UpdateType.Poll,
            // { PollAnswer: not null } => UpdateType.PollAnswer,
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