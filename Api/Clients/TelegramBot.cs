using System.Text.Json;
using TgCore.Api.Data;
using TgCore.Api.Enums;
using TgCore.Api.Helpers;
using TgCore.Api.Interfaces;
using TgCore.Api.Methods;
using TgCore.Api.Types;
using TgCore.Sdk.Looping;

namespace TgCore.Api.Clients;

public sealed class TelegramBot
{
    private readonly List<Func<Update, Task>> _updateHandlers = new();
    private readonly List<Func<Exception, Update?, Task>> _errorHandlers = new();
    private readonly List<BotLoop> _loops = new();

    private readonly TelegramClient _client;
    private CancellationTokenSource? _cts;
    private bool _isRunning;

    public bool IsRunning => _isRunning;
    public BotOptions Options { get; }

    public TelegramBot(BotOptions options)
    {
        Options = options;
        _client = new TelegramClient(options);
    }

    public async Task Run()
    {
        if (_isRunning) return;

        _cts = new CancellationTokenSource();
        _isRunning = true;

        try
        {
            await _client.StartReceiving(_updateHandlers, _errorHandlers, _loops, _cts.Token);
        }
        finally
        {
            _isRunning = false;
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

    public void RemoveUpdateHandler(Func<Update, Task> handler) => _updateHandlers.Remove(handler);
    public void RemoveErrorHandler(Func<Exception, Update?, Task> handler) => _errorHandlers.Remove(handler);
    
    public void AddLoop(BotLoop loop) => _loops.Add(loop);
    public void RemoveLoop(BotLoop loop) => _loops.Remove(loop);

    public async Task<Message?> SendText(long chatId, string text, IKeyboardMarkup? keyboard = null, long? replyId = null, 
        ParseMode? parseMode = null)
    {
        try
        {
            var message = await _client.CallAsync<Message>(TelegramMethods.SendMessage, new
            {
                chat_id = chatId,
                text = text,
                parse_mode = BotHelper.GetParseModeName(parseMode ?? Options.DefaultParseMode),
                reply_to_message_id = replyId,
                allow_sending_without_reply = true,
                reply_markup = keyboard
            });
        
            return message;
        }
        catch (Exception ex)
        {
            await Task.WhenAll(_errorHandlers.Select(h => h(ex, null)));
            return null;
        }
    }

    public async Task<Message?> EditText(long chatId, long messageId, string text,
        IKeyboardMarkup? keyboard = null, long? replyId = null, ParseMode? parseMode = null)
    {
        try
        {
            var message = await _client.CallAsync<Message>(TelegramMethods.EditMessageText, new
            {
                chat_id = chatId,
                message_id = messageId,
                text = text,
                parse_mode = BotHelper.GetParseModeName(parseMode ?? Options.DefaultParseMode),
                reply_to_message_id = replyId,
                allow_sending_without_reply = true,
                reply_markup = keyboard
            });
        
            return message;
        }
        catch (Exception ex)
        {
            await Task.WhenAll(_errorHandlers.Select(h => h(ex, null)));
            return null;
        }
    }

    public async Task<bool> DeleteMessage(long chatId, long messageId)
    {
        try
        {
            return await _client.CallAsync<bool>(TelegramMethods.DeleteMessage, new
            {
                chat_id = chatId,
                message_id = messageId
            });
        }
        catch (Exception ex)
        {
            await Task.WhenAll(_errorHandlers.Select(h => h(ex, null)));
            return false;
        }
    }

    public async Task<bool> AnswerCallback(string callbackId, string? text = null, bool showAlert = false)
    {
        try
        {
            return await _client.CallAsync<bool>(TelegramMethods.AnswerCallbackQuery, new
            {
                callback_query_id = callbackId,
                text = text,
                show_alert = showAlert
            });
        }
        catch (Exception ex)
        {
            await Task.WhenAll(_errorHandlers.Select(h => h(ex, null)));
            return false;
        }
    }

    public async Task<(bool Ok, T? Result)> SendRequest<T>(string method, object? body = null, JsonSerializerOptions? options = null)
    {
        try
        {
            return (true, await _client.CallAsync<T>(method, body, options));
        }
        catch (Exception ex)
        {
            await Task.WhenAll(_errorHandlers.Select(h => h(ex, null)));
            return (false, default);
        }
    }
}