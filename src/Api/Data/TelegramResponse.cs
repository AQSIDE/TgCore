namespace TgCore.Api.Data;

public sealed class TelegramResponse<T>
{
    [JsonPropertyName("ok")]
    public bool Ok { get; set; }
    
    [JsonPropertyName("error_code")]
    public int ErrorCode { get; set; }
    
    [JsonPropertyName("result")]
    public T? Result { get; set; }
    
    [JsonPropertyName("description")]
    public string? Description { get; set; }
}