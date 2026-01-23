namespace TgCore.Api.Data;

public class RateLimitOptions
{
    public IRateLimitModule Module { get; set; }

    public RateLimitOptions(IRateLimitModule module)
    {
        Module = module;
    }
}