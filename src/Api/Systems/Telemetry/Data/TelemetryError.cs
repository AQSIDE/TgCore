using TgCore.Api.Exceptions;

namespace TgCore.Api.Systems.Telemetry.Data;

public sealed class TelemetryError
{
    public Exception Exception { get; }
    public TelegramApiException? ApiException { get; }
    public DateTime CreateDate { get; }
    
    public bool IsTelegram => ApiException != null;
    public bool IsLocal => ApiException == null;

    public TelemetryError(Exception exception, TelegramApiException? apiException = null)
    {
        ApiException = apiException;
        Exception = exception;
        
        CreateDate = DateTime.Now;
    }
}

public sealed class TelemetryErrorDto
{
    public Exception Exception { get; init; }
    public TelegramApiException? ApiException { get; init; }
    public DateTime CreateDate { get; init; }
}

public sealed class LocalErrorStats
{
    public string ExceptionType { get; init; }
    public string? Method { get; init; }
    public string Message { get; init; }
    public int Count { get; init; }

    public LocalErrorStats(string exceptionType, string method, string message, int count)
    {
        ExceptionType = exceptionType;
        Method = method;
        Message = message;
        Count = count;
    }
}

public sealed class ApiErrorStats
{
    public int? ErrorCode { get; init; }
    public string? Method { get; init; }
    public string? Description { get; init; }
    public int Count { get; init; }

    public ApiErrorStats(int? errorCode, string method, string message, int count)
    {
        ErrorCode = errorCode;
        Method = method;
        Description = message;
        Count = count;
    }
}