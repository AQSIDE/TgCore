namespace TgCore.Api.Requests;

public sealed class RequestResponse<TResponse>
{
    public bool Ok { get; }
    public TResponse? Result { get; }
    public Exception? Error { get; }

    private RequestResponse(bool ok, TResponse? result, Exception? error)
    {
        Ok = ok;
        Result = result;
        Error = error;
    }
    
    internal static RequestResponse<TResponse> Success(TResponse result)
    {
        if (result is null)
            throw new ArgumentNullException(nameof(result));

        return new(true, result, null);
    }
    
    internal static RequestResponse<TResponse> UnSuccess()
    {
        return new(false, default, null);
    }

    internal static RequestResponse<TResponse> Fail(Exception ex)
    {
        if (ex is null)
            throw new ArgumentNullException(nameof(ex));

        return new(false, default, ex);
    }
}