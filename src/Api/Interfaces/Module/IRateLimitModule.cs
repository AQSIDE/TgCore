namespace TgCore.Api.Interfaces.Module;

public interface IRateLimitModule
{
    ValueTask WaitAsync(CancellationToken ct = default);
}