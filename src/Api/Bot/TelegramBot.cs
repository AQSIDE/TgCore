namespace TgCore.Api.Bot;

public sealed class TelegramBot
{
    private readonly List<Func<Update, Task>> _updateHandlers = new();
    private readonly List<Func<Exception, Task>> _errorHandlers = new();
    private readonly List<IBotLoop> _loops = new();
    private readonly BotRuntime _runtime;
    
    private CancellationTokenSource? _cts;
    private bool _isRunning;

    public bool IsRunning => _isRunning;
    public ITelegramClient Client => Options.Client;
    public MessageService Message { get; }
    public BotTaskLoop MainLoop { get; }
    public BotOptions Options { get; }

    public TelegramBot(BotOptions options)
    {
        Options = options;
        _runtime = new BotRuntime(options.UpdateReceiver, options.LoopRunner);

        Message = new MessageService(this);
        MainLoop = new BotTaskLoop(bot: this);
        _loops.Add(MainLoop);
    }

    public async Task Run(CancellationToken ct = default)
    {
        if (_isRunning) throw new InvalidOperationException("Bot is already running.");

        _cts = CancellationTokenSource.CreateLinkedTokenSource(ct);
        _isRunning = true;

        try
        {
            await _runtime.RunAsync(
                _updateHandlers.AsReadOnly(), 
                _errorHandlers.AsReadOnly(),
                _loops.AsReadOnly(), 
                _cts.Token);
        }
        catch (OperationCanceledException) when (ct.IsCancellationRequested)
        {
        }
        catch (Exception ex)
        {
            await AddException(ex);
            await Stop();
            throw;
        }
    }

    public Task Stop()
    {
        if (!_isRunning || _cts == null) 
            throw new InvalidOperationException("Bot is not running.");

        _cts.Cancel();
        _cts.Dispose();
        _cts = null;
        _isRunning = false;
        
        return Task.CompletedTask;
    }

    public TelegramBot AddUpdateHandler(params Func<Update, Task>[] handlers)
    {
        _updateHandlers.AddRange(handlers);
        return this;
    }

    public TelegramBot AddErrorHandler(params Func<Exception, Task>[] handlers)
    {
        _errorHandlers.AddRange(handlers);
        return this;
    }

    public TelegramBot AddLoop(IBotLoop[] loops)
    {
        foreach (var loop in loops)
        {
            if (!_loops.Contains(loop))
                _loops.Add(loop);
        }
        
        return this;
    } 
    
    public async Task AddException(Exception exception)
    {
        if (_errorHandlers.Count > 0)
            await Task.WhenAll(_errorHandlers.Select(f => f.Invoke(exception)));
    }
}