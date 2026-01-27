using TgCore.Api.Types.File;

namespace TgCore.Api.Types;

public sealed class Message
{
    [JsonPropertyName("message_id")]
    public long Id { get; set; }
    
    [JsonPropertyName("message_thread_id")]
    public long? MessageThreadId { get; set; }
    
    // TODO: create type
    [JsonPropertyName("direct_messages_topic")]
    public object? DirectMessagesTopic { get; set; }
    
    [JsonPropertyName("from")]
    public User? From { get; set; }
    
    [JsonPropertyName("sender_chat")]
    public Chat? SenderChat { get; set; }
    
    [JsonPropertyName("sender_boost_count")]
    public int? SenderBoostCount { get; set; }
    
    [JsonPropertyName("sender_business_bot")]
    public User? SenderBusinessBot { get; set; }
    
    [JsonPropertyName("date")]
    public long Date { get; set; }
    
    [JsonPropertyName("business_connection_id")]
    public string? BusinessConnectionId { get; set; }
    
    [JsonPropertyName("chat")]
    public Chat? Chat { get; set; }
    
    // TODO: create type
    [JsonPropertyName("forward_origin")]
    public object? ForwardOrigin { get; set; }
    
    [JsonPropertyName("is_topic_message")]
    public bool? IsTopicMessage { get; set; }
    
    [JsonPropertyName("is_automatic_forward")]
    public bool? IsAutomaticForward { get; set; }
    
    [JsonPropertyName("reply_to_message")]
    public Message? ReplyToMessage { get; set; }
    
    // TODO: create type
    [JsonPropertyName("external_reply")]
    public object? ExternalReply { get; set; }
    
    // TODO: create type
    [JsonPropertyName("quote")]
    public object? Quote { get; set; }
    
    // TODO: create type
    [JsonPropertyName("reply_to_story")]
    public object? ReplyToStory { get; set; }
    
    [JsonPropertyName("reply_to_checklist_task_id")]
    public int? ReplyToChecklistTaskId { get; set; }
    
    [JsonPropertyName("via_bot")]
    public User? ViaBot { get; set; }
    
    [JsonPropertyName("edit_date")]
    public long? EditDate { get; set; }
    
    [JsonPropertyName("text")]
    public string? Text { get; set; }
    
    [JsonPropertyName("entities")]
    public MessageEntity[]? Entities { get; set; }
    
    // TODO: create type
    [JsonPropertyName("link_preview_options")]
    public object? LinkPreviewOptions { get; set; }
    
    // TODO: create type
    [JsonPropertyName("suggested_post_info")]
    public object? SuggestedPostInfo { get; set; }
    
    [JsonPropertyName("effect_id")]
    public string? EffectId { get; set; }
    
    [JsonPropertyName("animation")]
    public Animation? Animation { get; set; }
    
    [JsonPropertyName("audio")]
    public Audio? Audio { get; set; }
    
    [JsonPropertyName("document")]
    public Document? Document { get; set; }
    
    // TODO: create type
    [JsonPropertyName("paid_media")]
    public object? PaidMedia { get; set; }
    
    [JsonPropertyName("photo")]
    public PhotoSize[]? Photo { get; set; }
    
    [JsonPropertyName("sticker")]
    public Sticker? Sticker { get; set; }
    
    // TODO: create type
    [JsonPropertyName("story")]
    public object? Story { get; set; }
    
    [JsonPropertyName("video")]
    public Video? Video { get; set; }
    
    // TODO: create type
    [JsonPropertyName("video_note")]
    public Video? VideoNote { get; set; }
    
    [JsonPropertyName("voice")]
    public Voice? Voice { get; set; }
    
    [JsonPropertyName("caption")]
    public string? Caption { get; set; }
    
    [JsonPropertyName("caption_entities")]
    public MessageEntity[]? CaptionEntities { get; set; }
    
    // TODO: create type
    [JsonPropertyName("checklist")]
    public object? Checklist { get; set; }
    
    // TODO: create type
    [JsonPropertyName("contact")]
    public object? Contact { get; set; }
    
    // TODO: create type
    [JsonPropertyName("dice")]
    public object? Dice { get; set; }
    
    // TODO: create type
    [JsonPropertyName("game")]
    public object? Game { get; set; }
    
    // TODO: create type
    [JsonPropertyName("poll")]
    public object? Poll { get; set; }
    
    // TODO: create type
    [JsonPropertyName("venue")]
    public object? Venue { get; set; }
    
    // TODO: create type
    [JsonPropertyName("location")]
    public object? Location { get; set; }
    
    [JsonPropertyName("new_chat_members")]
    public User[]? NewChatMembers { get; set; }
    
    [JsonPropertyName("left_chat_member")]
    public User? LeftChatMember { get; set; }
    
    [JsonPropertyName("new_chat_title")]
    public string? NewChatTitle { get; set; }
    
    [JsonPropertyName("new_chat_photo")]
    public PhotoSize[]? NewChatPhoto { get; set; }
    
    // TODO: create type
    [JsonPropertyName("invoice")]
    public object? Invoice { get; set; }
    
    // TODO: create type
    [JsonPropertyName("successful_payment")]
    public object? SuccessfulPayment { get; set; }
    
    // TODO: create type
    [JsonPropertyName("refunded_payment")]
    public object? RefundedPayment { get; set; }
    
    // TODO: create type
    [JsonPropertyName("users_shared")]
    public object? UsersShared { get; set; }
    
    // TODO: create type
    [JsonPropertyName("chat_shared")]
    public object? ChatShared { get; set; }
    
    // TODO: create type
    [JsonPropertyName("gift")]
    public object? Gift { get; set; }
    
    // TODO: create type
    [JsonPropertyName("unique_gift")]
    public object? UniqueGift { get; set; }
    
    // TODO: create type
    [JsonPropertyName("gift_upgrade_sent")]
    public object? GiftUpgradeSent { get; set; }
    
    [JsonPropertyName("connected_website")]
    public string? ConnectedWebsite { get; set; }
    
    // TODO: create type
    [JsonPropertyName("passport_data")]
    public object? PassportData { get; set; }
    
    // TODO: create type
    [JsonPropertyName("boost_added")]
    public object? BoostAdded { get; set; }
    
    // TODO: create type
    [JsonPropertyName("chat_background_set")]
    public object? ChatBackgroundSet { get; set; }
    
    // TODO: create type
    [JsonPropertyName("giveaway_created")]
    public object? GiveawayCreated { get; set; }
    
    // TODO: create type
    [JsonPropertyName("giveaway")]
    public object? Giveaway { get; set; }
    
    // TODO: create type
    [JsonPropertyName("giveaway_winners")]
    public object? GiveawayWinners { get; set; }
    
    // TODO: create type
    [JsonPropertyName("giveaway_completed")]
    public object? GiveawayCompleted { get; set; }
    
    // TODO: create type
    [JsonPropertyName("video_chat_scheduled")]
    public object? VideoChatScheduled { get; set; }
    
    // TODO: create type
    [JsonPropertyName("video_chat_started")]
    public object? VideoChatStarted { get; set; }
    
    // TODO: create type
    [JsonPropertyName("video_chat_ended")]
    public object? VideoChatEnded { get; set; }
    
    // TODO: create type
    [JsonPropertyName("video_chat_participants_invited")]
    public object? VideoChatParticipantsInvited { get; set; }
    
    // TODO: create type
    [JsonPropertyName("web_app_data")]
    public object? WebAppData { get; set; }
    
    [JsonPropertyName("reply_markup")]
    public InlineKeyboard? ReplyMarkup { get; set; }
    
    public bool IsEmpty => string.IsNullOrEmpty(Text) && 
                           Photo == null && 
                           Video == null && 
                           Audio == null && 
                           Document == null && 
                           Animation == null;
    public bool IsReply => ReplyToMessage != null;
}