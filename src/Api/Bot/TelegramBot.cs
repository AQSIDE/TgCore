using System.Diagnostics;
using TgCore.Api.Exceptions;
using TgCore.Api.Systems.Telemetry;
using TgCore.Api.Systems.Telemetry.Data;
using TgCore.Diagnostics.Debugger;
using Debug = TgCore.Diagnostics.Debugger.Debug;

namespace TgCore.Api.Bot;

public sealed class TelegramBot
{
    private readonly List<Func<Update, CancellationToken, Task>> _updateHandlers = new();
    private readonly List<Func<Exception, CancellationToken, Task>> _errorHandlers = new();
    private readonly List<IBotLoop> _loops = new();
    
    private TelemetrySystem _telemetry;
    private BotRuntime? _runtime;

    private CancellationTokenSource? _cts;
    private bool _isRunning;
    
    public User? Me { get; set; }
    public DateTime StartTime { get; private set; }
    public ModulesConfigurator Modules { get; }
    public TelegramRequests Requests { get; }
    public BotTaskLoop MainLoop { get; }
    public BotOptions Options { get; }
    
    public bool IsRunning => _isRunning;
    internal ITelegramClient Client => Options.Client;
    public TelemetrySystem Telemetry => _telemetry;

    public TelegramBot(BotOptions options)
    {
        Options = options;

        Requests = new TelegramRequests(this);
        MainLoop = new BotTaskLoop(bot: this);
        Modules = new ModulesConfigurator(this);
        
        _telemetry ??= new TelemetrySystem(this, options.InitialTelemetryConfig);
        _telemetry.Enabled = options.InitialUseTelemetry;

        _loops.Add(MainLoop);
    }

    public async Task Run(BotStartOptions? options = null, CancellationToken ct = default)
    {
        if (_isRunning) throw new InvalidOperationException("Bot is already running.");

        _cts = CancellationTokenSource.CreateLinkedTokenSource(ct);
        options ??= new BotStartOptions();
        _isRunning = true;
        
        StartTime = DateTime.UtcNow;

        if (options.NeedHandshake)
            await StartHandshake(ct);

        _runtime ??= new BotRuntime(Options.UpdateReceiver!, Options.LoopRunner!);

        try
        {
            await _runtime.RunAsync(
                _updateHandlers.AsReadOnly(),
                _errorHandlers.AsReadOnly(),
                _loops.AsReadOnly(),
                _telemetry,
                _cts.Token);
        }
        catch (OperationCanceledException) when (ct.IsCancellationRequested)
        {
        }
        catch (Exception ex)
        {
            await AddException(ex, ct);
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
        _runtime = null;
        _cts = null;
        _isRunning = false;

        return Task.CompletedTask;
    }

    public async Task Restart(BotStartOptions? options = null, CancellationToken ct = default)
    {
        await Stop();
        await Run(options, ct);
    }

    public TelegramBot AddUpdateHandler(params Func<Update, CancellationToken, Task>[] handlers)
    {
        _updateHandlers.AddRange(handlers);
        return this;
    }

    public TelegramBot AddErrorHandler(params Func<Exception, CancellationToken, Task>[] handlers)
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

    public static TelegramBotBuilder Create(ITelegramClient client)
    {
        return new TelegramBotBuilder(client);
    }

    public static TelegramBotBuilder Create(string token)
    {
        var client = new TelegramClient(token);
        return new TelegramBotBuilder(client);
    }

    public static TelegramBot Default(string token)
    {
        var client = new TelegramClient(token);
        return new TelegramBotBuilder(client).Build();
    }

    public async Task AddException(Exception exception, CancellationToken ct = default)
    {
        var sw = Stopwatch.StartNew();
        try
        {
            if (_errorHandlers.Count > 0)
                await Task.WhenAll(_errorHandlers.Select(f => f.Invoke(exception, ct)));
        }
        finally
        {
            sw.Stop();
            _telemetry.Update(s => s.AddError(new TelemetryError(exception, exception as TelegramApiException)));
            _telemetry.Update(s => s.AddErrorHandlerLatency(sw.ElapsedMilliseconds));
        }
    }

    private async Task StartHandshake(CancellationToken ct = default)
    {
        try
        {
            var me = await Requests.GetMe(ct);

            if (me.Result == null)
            {
                Debug.Console.LogError("Telegram API returned empty bot information",
                    new LogOptions { UseFullDate = true, Category = "Handshake" });
                throw new InvalidOperationException("Telegram API returned empty bot information");
            }

            Me = me.Result;

            Debug.Console.LogInfo($"Bot successfully connected: {me.Result.FirstName} (@{me.Result.Username}) [ID: {me.Result.Id}]",
                new LogOptions { UseFullDate = true, Category = "Handshake" });
        }
        catch (Exception ex)
        {
            Debug.Console.LogFatal($"Error verifying bot connection: {ex.Message}", new LogOptions { UseFullDate = true, Category = "Handshake" });
            _isRunning = false;
            throw;
        }
    }
}