namespace TgCore.Api.Interfaces;

public interface IRateLimitModule
{
    ValueTask WaitAsync(CancellationToken ct = default);
}