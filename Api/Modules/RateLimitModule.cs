namespace TgCore.Api.Modules;

public class RateLimiter : IRateLimitModule
{
    private readonly int _requestsPerSecond;  
    private readonly int _maxBurstSize;      
    private double _availableTokens;       
    private readonly double _refillRatePerSecond; 
    private DateTime _lastRefillTime;  
    
    private readonly SemaphoreSlim _lock = new(1, 1);

    public RateLimiter(int requestsPerSecond = 20, int maxBurstSize = 25)
    {
        _requestsPerSecond = requestsPerSecond;
        _maxBurstSize = maxBurstSize;
        _availableTokens = maxBurstSize;
        _refillRatePerSecond = requestsPerSecond;
        _lastRefillTime = DateTime.UtcNow;
    }
    
    public async ValueTask WaitAsync(CancellationToken cancellationToken = default)
    {
        while (true)
        {
            cancellationToken.ThrowIfCancellationRequested();

            await _lock.WaitAsync(cancellationToken);
            try
            {
                RefillAvailableTokens();

                if (_availableTokens >= 1)
                {
                    _availableTokens--;
                    return;
                }
            }
            finally
            {
                _lock.Release();
            }
            
            await Task.Delay(50, cancellationToken);
        }
    }
    
    private void RefillAvailableTokens()
    {
        var currentTime = DateTime.UtcNow;
        var timePassedSeconds = (currentTime - _lastRefillTime).TotalSeconds;
        _lastRefillTime = currentTime;
        
        _availableTokens += timePassedSeconds * _refillRatePerSecond;
        
        if (_availableTokens > _maxBurstSize)
            _availableTokens = _maxBurstSize;
    }
}