using System.Collections.Concurrent;
using TgCore.Api.Clients;
using TgCore.Api.Interfaces;
using TgCore.Api.Types;
using TgCore.Sdk.Data;
using TgCore.Sdk.Data.Context;
using TgCore.Sdk.Interfaces;

namespace TgCore.Sdk.Services;

public class BotMainMessageService
{
    private readonly ConcurrentDictionary<long, MainMessageContext> _messages = new();
    private readonly TelegramBot _bot;

    public BotMainMessageService(TelegramBot bot)
    {
        _bot = bot;
    }

    public async Task SendMessage(long userId, string text, IKeyboardMarkup? keyboard = null, long? replyId = null,
        long? otherMessageId = null)
    {
        try
        {
            var message = await _bot.Message.SendText(userId, text, keyboard, replyId);
            
            if (message != null)
                _messages[userId] = new MainMessageContext(message, keyboard);

            if (otherMessageId != null && message != null)
                await DeleteOtherMessages(userId, message.Id, otherMessageId.Value);
        }
        catch (Exception ex)
        {
            await _bot.AddException(ex, null);
        }
    }

    public async Task<bool> DeleteMessage(long userId)
    {
        try
        {
            if (_messages.TryGetValue(userId, out var context))
            {
                await _bot.Message.DeleteMessage(userId, context.Message.Id);
                _messages.TryRemove(userId, out _);

                return true;
            }

            return false;
        }
        catch (Exception ex)
        {
            await _bot.AddException(ex, null);
            return false;
        }
    }

    public async Task SendAndDelete(long userId, string text, IKeyboardMarkup? keyboard = null, long? replyId = null,
        long? otherMessageId = null)
    {
        try
        {
            await DeleteMessage(userId);
            await SendMessage(userId, text, keyboard: keyboard, replyId: replyId, otherMessageId: otherMessageId);
        }
        catch (Exception ex)
        {
            await _bot.AddException(ex, null);
        }
    }

    public async Task UpdateOrCreate(long userId, string text, IKeyboardMarkup? keyboard = null, long? replyId = null,
        long? otherMessageId = null)
    {
        try
        {
            if (_messages.TryGetValue(userId, out var context))
            {
                if (context.Message.Text != text && context.Keyboard != keyboard)
                    await _bot.Message.EditText(userId, context.Message.Id, text, keyboard);
            }
            else
            {
                await SendMessage(userId, text, keyboard: keyboard, replyId: replyId);
            }

            if (context?.Message != null && otherMessageId != null)
                await DeleteOtherMessages(userId, context.Message.Id, otherMessageId.Value);
        }
        catch (Exception ex)
        {
            await _bot.AddException(ex, null);
        }
    }

    public MainMessageContext? GetMessage(long userId)
    {
        _messages.TryGetValue(userId, out var context);
        return context;
    }

    public async Task Remove(long fromId, bool deleteMainMessage = true)
    {
        try
        {
            if (deleteMainMessage)
                await DeleteMessage(fromId);

            _messages.TryRemove(fromId, out _);
        }
        catch (Exception ex)
        {
            await _bot.AddException(ex, null);
        }
    }

    private async Task DeleteOtherMessages(long userId, long mainMessageId, long otherMessageId)
    {
        if (otherMessageId != mainMessageId)
            await _bot.Message.DeleteMessage(userId, otherMessageId);
    }
}