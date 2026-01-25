using TgCore.Api.Bot;
using TgCore.Api.Looping;
using TgCore.Sdk.Data;
using TgCore.Sdk.Interfaces;

namespace TgCore.Sdk.Dispatching;

public class BotRequestDispatcher
{
    private readonly List<RequestTask> _requests = new();
    private List<RequestTask> _toExecute = new();
    private List<RequestTask> _removed = new();

    private readonly object _lock = new();
    private readonly TelegramBot? _bot;
    private readonly BotTaskLoop _loop;

    public uint MaxRequestsPerInterval { get; set; }

    public BotRequestDispatcher(BotTaskLoop loop, TelegramBot? bot = null, double interval = 1,
        uint maxRequestsPerInterval = 20)
    {
        _loop = loop;
        _bot = bot;

        interval = Math.Clamp(interval, 1, int.MaxValue);
        MaxRequestsPerInterval = maxRequestsPerInterval;

        _loop.AddRepeatingTask(TimeSpan.FromSeconds(interval), Execute, DateTime.Now.AddSeconds(2));
    }

    public void Add(RequestTask request)
    {
        lock (_lock)
        {
            _requests.Add(request);
        }
    }

    public async Task Execute()
    {
        lock (_lock)
        {
            if (_requests.Count == 0 || MaxRequestsPerInterval <= 0) return;

            _toExecute = _requests
                .OrderBy(r => r.Priority)
                .ThenBy(r => r.CreatedAt)
                .Take((int)MaxRequestsPerInterval)
                .ToList();

            if (_toExecute.Count == 0) return;

            foreach (var request in _toExecute)
            {
                _requests.Remove(request);
                _removed.Add(request);
            }
        }

        try
        {
            await Task.WhenAll(_toExecute.Select(t => t.Execute()));
        }
        catch (Exception ex)
        {
            if (_bot != null)
                await _bot.AddException(ex);

            var failedRequests = 0;
            lock (_lock)
            {
                foreach (var request in _removed)
                {
                    if (request.RetryCount < request.MaxRetries)
                    {
                        request.RetryCount++;
                        _requests.Add(request);
                    }
                    else
                    {
                        failedRequests++;
                    }
                }
            }

            if (failedRequests > 0 && _bot != null)
                await _bot.AddException(new Exception($"{failedRequests} requests failed after retries"));
        }
        finally
        {
            _toExecute.Clear();
            _removed.Clear();
        }
    }
}