namespace TgCore.Api.Data;

internal sealed class TelegramResponse<T>
{
    [JsonPropertyName("ok")]
    public bool Ok { get; set; }
    
    [JsonPropertyName("result")]
    public T Result { get; set; }
    
    [JsonPropertyName("description")]
    public string? Description { get; set; }
}