namespace TgCore.Api.Systems.Telemetry.Data;

public sealed class TelemetryRequest
{
    public string Method { get; }
    public double LatencyMs { get; }
    public bool IsSuccess { get; }
    public string? ErrorMessage { get; }
    public DateTime CreateDate { get; }

    public TelemetryRequest(string method, double latencyMs, bool isSuccess, string? errorMessage = null)
    {
        Method = method;
        LatencyMs = latencyMs;
        IsSuccess = isSuccess;
        ErrorMessage = errorMessage;
        
        CreateDate = DateTime.Now;
    }
}

public sealed class TelemetryRequestDto
{
    public string Method { get; init; }
    public double LatencyMs { get; init; }
    public bool IsSuccess { get; init; }
    public string? ErrorMessage { get; init; }
    public DateTime CreateDate { get; init; }
}