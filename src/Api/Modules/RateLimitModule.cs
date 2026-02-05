namespace TgCore.Api.Modules;

public class RateLimitModule : IRateLimitModule
{
    private readonly TimeSpan _interval;
    private readonly int _maxTokens;
    private readonly int _maxBurst;
    
    private int _tokens;
    private int _burstUsed;
    private DateTime _nextRefill;
    
    private readonly SemaphoreSlim _lock = new(1, 1);

    public RateLimitModule(int maxTokens = 10, int maxBurst = 3, TimeSpan? interval = null)
    {
        _maxTokens = maxTokens;
        _maxBurst = maxBurst;
        _interval = interval ?? TimeSpan.FromSeconds(1);
        _tokens = maxTokens;
        _nextRefill = DateTime.UtcNow;
    }

    public async ValueTask WaitAsync(CancellationToken ct = default)
    {
        TimeSpan delay;
        
        while (true)
        {
            ct.ThrowIfCancellationRequested();
            
            await _lock.WaitAsync(ct);
            try
            {
                var now = DateTime.UtcNow;
                if (now >= _nextRefill)
                {
                    _tokens = _maxTokens;
                    _burstUsed = 0;
                    _nextRefill = now + _interval;
                }

                if (_tokens > 0 && _burstUsed < _maxBurst)
                {
                    _tokens--;
                    _burstUsed++;
                    return;
                }

                delay = _nextRefill - now;
            }
            finally
            {
                _lock.Release();
            }

            if (delay > TimeSpan.Zero) 
                await Task.Delay(delay, ct);
        }
    }
}