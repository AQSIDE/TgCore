namespace TgCore.Api.Types.Business;

public sealed class BusinessBotRights
{
    [JsonPropertyName("can_reply")] 
    public bool CanReply { get; set; }
    
    [JsonPropertyName("can_read_messages")] 
    public bool CanReadMessages { get; set; }
    
    [JsonPropertyName("can_delete_sent_messages")] 
    public bool CanDeleteSentMessages { get; set; }
    
    [JsonPropertyName("can_delete_all_messages")] 
    public bool CanDeleteAllMessages { get; set; }
    
    [JsonPropertyName("can_edit_name")] 
    public bool CanEditName { get; set; }
    
    [JsonPropertyName("can_edit_bio")] 
    public bool CanEditBio { get; set; }
    
    [JsonPropertyName("can_edit_profile_photo")] 
    public bool CanEditProfilePhoto { get; set; }
    
    [JsonPropertyName("can_edit_username")] 
    public bool CanEditUsername { get; set; }
    
    [JsonPropertyName("can_change_gift_settings")] 
    public bool CanChangeGiftSettings { get; set; }
    
    [JsonPropertyName("can_view_gifts_and_stars")] 
    public bool CanViewGiftsAndStars { get; set; }
    
    [JsonPropertyName("can_convert_gifts_to_stars")] 
    public bool CanConvertGiftsToStars { get; set; }
    
    [JsonPropertyName("can_transfer_and_upgrade_gifts")] 
    public bool CanTransferAndUpgradeGifts { get; set; }
    
    [JsonPropertyName("can_transfer_stars")] 
    public bool CanTransferStars { get; set; }
    
    [JsonPropertyName("can_manage_stories")] 
    public bool CanManageStories { get; set; }
}