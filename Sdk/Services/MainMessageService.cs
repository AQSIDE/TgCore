using System.Collections.Concurrent;
using TgCore.Api.Clients;
using TgCore.Api.Interfaces;
using TgCore.Api.Types;

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
        var message = await _bot.SendText(userId, text, keyboard, replyId);
        if (message != null)
            _messages[userId] = new MainMessageContext(message, keyboard);

        if (otherMessageId != null && message != null)
            await DeleteOtherMessages(userId, message.Id, otherMessageId.Value);
    }

    public async Task<bool> DeleteMessage(long userId)
    {
        if (_messages.TryGetValue(userId, out var context))
        {
            await _bot.DeleteMessage(userId, context.Message.Id);
            _messages.TryRemove(userId, out _);

            return true;
        }

        return false;
    }

    public async Task SendAndDelete(long userId, string text, IKeyboardMarkup? keyboard = null, long? replyId = null,
        long? otherMessageId = null)
    {
        await DeleteMessage(userId);
        await SendMessage(userId, text, keyboard: keyboard, replyId: replyId, otherMessageId: otherMessageId);
    }

    public async Task UpdateOrCreate(long userId, string text, IKeyboardMarkup? keyboard = null, long? replyId = null,
        long? otherMessageId = null)
    {
        if (_messages.TryGetValue(userId, out var context))
        {
            if (context.Message.Text != text && context.Keyboard != keyboard)
                await _bot.EditText(userId, context.Message.Id, text, keyboard, replyId);
        }
        else
        {
            await SendMessage(userId, text, keyboard: keyboard, replyId: replyId);
        }

        if (context?.Message != null && otherMessageId != null)
            await DeleteOtherMessages(userId, context.Message.Id, otherMessageId.Value);
    }

    public MainMessageContext? GetMessage(long userId)
    {
        _messages.TryGetValue(userId, out var context);
        return context;
    }

    public async Task Remove(long fromId, bool deleteMainMessage = true)
    {
        if (deleteMainMessage)
            await DeleteMessage(fromId);

        _messages.TryRemove(fromId, out _);
    }

    private async Task DeleteOtherMessages(long userId, long mainMessageId, long otherMessageId)
    {
        if (otherMessageId != mainMessageId)
            await _bot.DeleteMessage(userId, otherMessageId);
    }
}