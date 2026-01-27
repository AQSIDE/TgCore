using TgCore.Api.Types.Base;

namespace TgCore.Api.Types;

public sealed class ChatFullInfo : ChatBase
{
    [JsonPropertyName("accent_color_id")]
    public int? AccentColorId { get; set; }
    
    [JsonPropertyName("max_reaction_count")]
    public int? MaxReactionCount { get; set; }
    
    // TODO: create type
    [JsonPropertyName("photo")]
    public object? ChatPhoto { get; set; }
    
    [JsonPropertyName("active_usernames")]
    public string[]? ActiveUsernames { get; set; }
    
    // TODO: create type
    [JsonPropertyName("birthdate")]
    public object? Birthdate { get; set; }
    
    // TODO: create type
    [JsonPropertyName("business_intro")]
    public object? BusinessIntro { get; set; }
    
    // TODO: create type
    [JsonPropertyName("business_location")]
    public object? BusinessLocation { get; set; }
    
    // TODO: create type
    [JsonPropertyName("business_opening_hours")]
    public object? BusinessOpeningHours { get; set; }
    
    [JsonPropertyName("personal_chat")]
    public Chat? PersonalChat { get; set; }
    
    [JsonPropertyName("parent_chat")]
    public Chat? ParentChat { get; set; }
    
    // TODO: create type
    [JsonPropertyName("available_reactions")]
    public object[]? AvailableReactions { get; set; }
    
    [JsonPropertyName("background_custom_emoji_id")]
    public string? BackgroundCustomEmojiId { get; set; }
    
    [JsonPropertyName("profile_accent_color_id")]
    public int? ProfileAccentColorId { get; set; }
    
    [JsonPropertyName("profile_background_custom_emoji_id")]
    public int? ProfileBackgroundCustomEmojiId { get; set; }
    
    [JsonPropertyName("emoji_status_custom_emoji_id")]
    public int? EmojiStatusCustomEmojiId { get; set; }
    
    [JsonPropertyName("emoji_status_expiration_date")]
    public int? EmojiStatusExpirationDate { get; set; }
    
    [JsonPropertyName("bio")]
    public string? Bio { get; set; }
    
    [JsonPropertyName("has_private_forwards")]
    public bool? HasPrivateForwards { get; set; }
    
    [JsonPropertyName("has_restricted_voice_and_video_messages")]
    public bool? HasRestrictedVoiceAndVideoMessages { get; set; }
    
    [JsonPropertyName("join_to_send_messages")]
    public bool? JoinToSendMessages { get; set; }
    
    [JsonPropertyName("join_by_request")]
    public bool? JoinByRequest { get; set; }
    
    [JsonPropertyName("description")]
    public string? Description { get; set; }
    
    [JsonPropertyName("invite_link")]
    public string? InviteLink { get; set; }
    
    [JsonPropertyName("pinned_message")]
    public Message? PinnedMessage { get; set; }
    
    // TODO: create type
    [JsonPropertyName("permissions")]
    public object? Permissions { get; set; }
    
    // TODO: create type
    [JsonPropertyName("accepted_gift_types")]
    public object? AcceptedGiftTypes { get; set; }
    
    [JsonPropertyName("can_send_paid_media")]
    public bool? CanSendPaidMedia { get; set; }
    
    [JsonPropertyName("slow_mode_delay")]
    public int? SlowModeDelay { get; set; }
    
    [JsonPropertyName("unrestrict_boost_count")]
    public int? UnrestrictBoostCount { get; set; }
    
    [JsonPropertyName("message_auto_delete_time")]
    public int? MessageAutoDeleteTime { get; set; }
    
    [JsonPropertyName("has_aggressive_anti_spam_enabled")]
    public bool? HasAggressiveAntiSpamEnabled { get; set; }
    
    [JsonPropertyName("has_hidden_members")]
    public bool? HasHiddenMembers { get; set; }
    
    [JsonPropertyName("has_protected_content")]
    public bool? HasProtectedContent { get; set; }
    
    [JsonPropertyName("has_visible_history")]
    public bool? HasVisibleHistory { get; set; }
    
    [JsonPropertyName("sticker_set_name")]
    public string? StickerSetName { get; set; }
    
    [JsonPropertyName("can_set_sticker_set")]
    public bool? CanSetStickerSet { get; set; }
    
    [JsonPropertyName("custom_emoji_sticker_set_name")]
    public string? CustomEmojiStickerSetName { get; set; }
    
    [JsonPropertyName("linked_chat_id")]
    public int? LinkedChatId { get; set; }
    
    // TODO: create type
    [JsonPropertyName("location")]
    public object? ChatLocation { get; set; }
    
    // TODO: create type
    [JsonPropertyName("rating")]
    public object? UserRating { get; set; }
    
    // TODO: create type
    [JsonPropertyName("unique_gift_colors")]
    public object? UniqueGiftColors { get; set; }
    
    [JsonPropertyName("paid_message_star_count")]
    public int? PaidMessageStarCount { get; set; }
}