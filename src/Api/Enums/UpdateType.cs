namespace TgCore.Api.Enums;

public enum UpdateType
{
    Unknown = 0,
    
    // -- Messages --
    Message,
    EditedMessage,
    CallbackQuery,
    ChannelPost,
    EditedChannelPost,
    
    // -- Business --
    BusinessConnection,
    BusinessMessage,
    EditedBusinessMessage,
    DeletedBusinessMessages,
    
    // -- Inline --
    InlineQuery,
    ChosenInlineResult,
    
    // -- Reaction & Status --
    MessageReaction,
    MessageReactionCount,
    ChatBoost,
    ChatBoostRemoved,
    
    // -- Polls --
    Poll,
    PollAnswer,
    
    // -- Pay -- 
    PreCheckoutQuery,
    ShippingQuery,
    PurchasedPaidMedia,
    
    // -- Admin & Members --
    ChatMember,
    MyChatMember,
    ChatJoinRequest
}