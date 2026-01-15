using System.Text.Json.Serialization;

namespace TgCore.Api.Types;

public sealed class User
{
    [JsonPropertyName("id")]
    public long Id { get; set; }
    
    [JsonPropertyName("first_name")]
    public string? FirstName { get; set; }
    
    [JsonPropertyName("username")]
    public string? Username { get; set; }
    
    [JsonPropertyName("language_code")]
    public string? LanguageCode { get; set; }
    
    [JsonPropertyName("is_premium")]
    public bool IsPremium { get; set; }
    
    [JsonPropertyName("is_bot")]
    public bool IsBot { get; set; }
}