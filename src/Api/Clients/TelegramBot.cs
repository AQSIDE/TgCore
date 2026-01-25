using TgCore.Api.Looping;
using TgCore.Api.Services.Message;

namespace TgCore.Api.Clients;

public sealed class TelegramBot
{
    private readonly List<Func<Update, Task>> _updateHandlers = new();
    private readonly List<Func<Exception, Update?, Task>> _errorHandlers = new();
    private readonly List<IBotLoop> _loops = new();

    private readonly TelegramClient _client;
    private CancellationTokenSource? _cts;
    private bool _isRunning;

    public bool IsRunning => _isRunning;
    internal TelegramClient Client => _client;
    public MessageService Message { get; }
    public BotTaskLoop MainLoop { get; }
    public BotOptions Options { get; }

    public TelegramBot(BotOptions options)
    {
        Options = options;
        _client = new TelegramClient(options);

        Message = new MessageService(this);
        MainLoop = new BotTaskLoop(bot: this);
        _loops.Add(MainLoop);
    }

    public async Task Run(CancellationToken externalToken = default)
    {
        if (_isRunning) return;

        _cts = CancellationTokenSource.CreateLinkedTokenSource(externalToken);
        _isRunning = true;

        try
        {
            await _client.StartReceiving(_updateHandlers, _errorHandlers, _loops, _cts.Token);
        }
        catch (OperationCanceledException) when (externalToken.IsCancellationRequested)
        {
        }
        catch (Exception ex)
        {
            await AddException(ex, null);
            Stop();
            throw;
        }
    }

    public void Stop()
    {
        if (!_isRunning || _cts == null) return;

        _cts.Cancel();
        _cts.Dispose();
        _cts = null;
        _isRunning = false;
    }

    public void AddUpdateHandler(Func<Update, Task> handler) => _updateHandlers.Add(handler);
    public void AddErrorHandler(Func<Exception, Update?, Task> handler) => _errorHandlers.Add(handler);
    public void AddLoop(IBotLoop loop)
    {
        if (!_loops.Contains(loop))
            _loops.Add(loop);
    } 
    
    public async Task AddException(Exception exception, Update? update)
    {
        if (_errorHandlers.Count > 0)
            await Task.WhenAll(_errorHandlers.Select(f => f.Invoke(exception, update)));
    }
}