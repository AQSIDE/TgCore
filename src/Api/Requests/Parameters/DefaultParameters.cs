namespace TgCore.Api.Requests.Parameters;

public class DefaultParameters : RequestParameters
{
    public bool? ProtectContent { get; set; }
    public int? DirectMessagesTopicId { get; set; }
    public string? MessageEffectId { get; set; }
    public bool? DisableNotification { get; set; }
    public bool? DisableWebPagePreview { get; set; }
    
    public override Dictionary<string, object> ToDictionary()
    {
        return new TelegramParametersBuilder()
            .Add("protect_content", ProtectContent)
            .Add("direct_messages_topic_id", DirectMessagesTopicId)
            .Add("disable_notification", DisableNotification)
            .Add("disable_web_page_preview", DisableWebPagePreview)
            .Add("message_effect_id", MessageEffectId)
            .AddDictionary(GetBase())
            .Build();
    }
}