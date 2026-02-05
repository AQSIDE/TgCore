namespace TgCore.Api.Exceptions;

public class TelegramApiException : Exception
{
    public string Message { get; }
    public int? ErrorCode { get; }
    public string? Description { get; }
    public string? Method { get; }

    public TelegramApiException(
        string message, 
        int? errorCode = null, 
        string? description = null, 
        string? method = null) 
        : base(message)
    {
        Message = message;
        ErrorCode = errorCode;
        Description = description;
        Method = method;
    }
}