namespace TgCore.Api.Runtime;

public class LongPollingReceiver : IUpdateReceiver
{
    private readonly UpdateType[] _allowedUpdates;
    private readonly ITelegramClient _client;
    
    private int _timeout;
    private int _limit;
    private long _offset;

    public LongPollingReceiver(ITelegramClient client, UpdateType[] allowedUpdates, 
        int limit = 100, 
        int timeout = 30,
        long startOffset = 0)
    {
        _allowedUpdates = allowedUpdates;
        _client = client;
        
        _limit = Math.Clamp(limit, 1, 100);
        _timeout = Math.Clamp(timeout, 0, 60);
        _offset = startOffset;
    }

    public async Task StartReceiving(
        IReadOnlyList<Func<Update, Task>> updateHandlers, 
        IReadOnlyList<Func<Exception, Task>> errorHandlers, 
        CancellationToken ct)
    {
        while (!ct.IsCancellationRequested)
        {
            try
            {
                var updates = await _client.CallAsync<Update[]>(TelegramMethods.GET_UPDATES, new
                {
                    offset = _offset,
                    timeout = _timeout,
                    limit = _limit,
                    allowed_updates = BotHelper.GetAllowedUpdatesNames(_allowedUpdates)
                });

                foreach (var update in updates)
                {
                    try
                    {
                        BotHelper.SetUpdateType(update);
                        await Task.WhenAll(updateHandlers.Select(f => f(update)));
                    }
                    catch (Exception ex)
                    {
                        await Task.WhenAll(errorHandlers.Select(f => f(ex)));
                    }
                    finally
                    {
                        _offset = update.Id + 1;
                    }
                }
            }
            catch (Exception ex)
            {
                await Task.WhenAll(errorHandlers.Select(f => f(ex)));
                await Task.Delay(5000, ct);
            }
        }
    }
}