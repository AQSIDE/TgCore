using System.Diagnostics;
using TgCore.Api.Exceptions;
using TgCore.Api.Systems.Telemetry;
using TgCore.Api.Systems.Telemetry.Data;

namespace TgCore.Api.Runtime;

public class LongPollingReceiver : IUpdateReceiver
{
    private readonly UpdateType[] _allowedUpdates;
    private readonly ITelegramClient _client;
    
    private readonly int _timeout;
    private readonly int _limit;
    
    private long _offset;

    public LongPollingReceiver(
        ITelegramClient client, 
        UpdateType[] allowedUpdates, 
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
        IReadOnlyList<Func<Update, CancellationToken, Task>> updateHandlers, 
        IReadOnlyList<Func<Exception, CancellationToken, Task>> errorHandlers, 
        TelemetrySystem telemetry,
        CancellationToken ct)
    {
        while (!ct.IsCancellationRequested)
        {
            try
            {
                var updates = await _client.CallAsync<Update[]>(TelegramMethods.GET_UPDATES, telemetry, new
                {
                    offset = _offset,
                    timeout = _timeout,
                    limit = _limit,
                    allowed_updates = BotHelper.GetAllowedUpdatesNames(_allowedUpdates)
                }, ct:ct);
                
                if (updates.Length == 0) continue;

                foreach (var update in updates)
                {
                    var swUpd = Stopwatch.StartNew();
                    try
                    {
                        BotHelper.SetUpdateType(update);
                        
                        await Task.WhenAll(updateHandlers.Select(f => f(update, ct)));
                    }
                    catch (Exception ex)
                    {
                        var swErr = Stopwatch.StartNew();
                        try
                        {
                            await Task.WhenAll(errorHandlers.Select(f => f(ex, ct)));
                        }
                        finally
                        {
                            swErr.Stop();
                            telemetry.Update(s => s.AddErrorHandlerLatency(swErr.ElapsedMilliseconds));
                            telemetry.Update(s => s.AddError(new TelemetryError(ex, ex as TelegramApiException)));
                        }
                    }
                    finally
                    {
                        swUpd.Stop();
                        
                        telemetry.Update(s =>  s.AddUpdate(update));
                        telemetry.Update(s => s.AddUpdateHandlerLatency(swUpd.ElapsedMilliseconds));
                        
                        _offset = update.Id + 1;
                    }
                }
            }
            catch (Exception ex)
            {
                var swErr = Stopwatch.StartNew();
                try
                {
                    await Task.WhenAll(errorHandlers.Select(f => f(ex, ct)));
                }
                finally
                {
                    swErr.Stop();
                    telemetry.Update(s => s.AddErrorHandlerLatency(swErr.ElapsedMilliseconds));
                    telemetry.Update(s => s.AddError(new TelemetryError(ex, ex as TelegramApiException)));
                }
            
                await Task.Delay(5000, ct);
            }
        }
    }
}