using TgCore.Api.Types.Business;
using TgCore.Api.Types.Poll;

namespace TgCore.Api.Types;

public sealed class Update
{
    [JsonPropertyName("update_id")] 
    public long Id { get; set; }

    // -- Messages --
    [JsonPropertyName("message")] 
    public Message? Message { get; set; }

    [JsonPropertyName("edited_message")] 
    public Message? EditedMessage { get; set; }

    [JsonPropertyName("channel_post")] 
    public Message? ChannelPost { get; set; }

    [JsonPropertyName("edited_channel_post")]
    public Message? EditedChannelPost { get; set; }
    
    // -- Business --
    [JsonPropertyName("business_connection")]
    public BusinessConnection? BusinessConnection { get; set; } 

    [JsonPropertyName("business_message")] 
    public Message? BusinessMessage { get; set; }

    [JsonPropertyName("edited_business_message")]
    public Message? EditedBusinessMessage { get; set; }

    [JsonPropertyName("deleted_business_messages")]
    public BusinessMessagesDeleted? DeletedBusinessMessages { get; set; }
    
    // -- Reactions & Boosts --
    [JsonPropertyName("message_reaction")] 
    public object? MessageReaction { get; set; }

    [JsonPropertyName("message_reaction_count")]
    public object? MessageReactionCount { get; set; }

    // -- Inline & Callbacks --
    [JsonPropertyName("inline_query")] 
    public InlineQuery? InlineQuery { get; set; }

    [JsonPropertyName("chosen_inline_result")]
    public ChosenInlineResult? ChosenInlineResult { get; set; }
    
    [JsonPropertyName("callback_query")] 
    public CallbackQuery? CallbackQuery { get; set; }
    
    // -- Payments --
    [JsonPropertyName("shipping_query")] 
    public object? ShippingQuery { get; set; }

    [JsonPropertyName("pre_checkout_query")]
    public object? PreCheckoutQuery { get; set; }

    [JsonPropertyName("purchased_paid_media")]
    public object? PurchasedPaidMedia { get; set; }

    // -- Polls --
    [JsonPropertyName("poll")] 
    public Poll.Poll? Poll { get; set; }

    [JsonPropertyName("poll_answer")] 
    public PollAnswer? PollAnswer { get; set; }
    
    // -- Admin & Members --
    [JsonPropertyName("chat_member")] 
    public object? ChatMember { get; set; }

    [JsonPropertyName("my_chat_member")] 
    public object? MyChatMember { get; set; }

    [JsonPropertyName("chat_join_request")]
    public object? ChatJoinRequest { get; set; }
    
    // -- Chat Boost --
    [JsonPropertyName("chat_boost")] 
    public object? ChatBoost { get; set; }

    [JsonPropertyName("chat_boost_removed")]
    public object? ChatBoostRemoved { get; set; }

    public UpdateType Type { get; set; }

    public User? GetFrom =>
        Message?.From ??
        EditedMessage?.From ??
        ChannelPost?.From ??
        EditedChannelPost?.From ??
        CallbackQuery?.From ??
        InlineQuery?.From ??
        ChosenInlineResult?.From;

    public Chat? GetChat =>
        Message?.Chat ??
        EditedMessage?.Chat ??
        ChannelPost?.Chat ??
        EditedChannelPost?.Chat ??
        CallbackQuery?.Chat ??
        InlineQuery?.Chat ??
        ChosenInlineResult?.Chat;

    public Message? GetMessage =>
        Message ??
        EditedMessage ??
        ChannelPost ??
        EditedChannelPost ??
        CallbackQuery?.Message;

    public long? FromId => GetFrom?.Id;
    
    /// <summary>
    /// Returns Chat.Id only. Does NOT return User.Id (use <see cref="FromId"/> for that).
    /// </summary>
    /// <remarks>
    /// This property extracts ONLY the chat ID from Chat object.
    /// It never returns user ID, even if Chat.Id equals User.Id in private chats.
    /// </remarks>
    public long? ChatId => GetChat?.Id;
    
    /// <summary>
    /// From ID with fallback to Chat ID
    /// </summary>
    public long? FromOrChatId => FromId ?? ChatId;
    public long? MessageId => GetMessage?.Id;
    public string? CallbackData => CallbackQuery?.Data;
    public string? Text => GetMessage?.Text;
}