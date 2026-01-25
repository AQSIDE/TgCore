namespace TgCore.Api.Interfaces.Client;

public interface ITelegramClient
{
    Task<T> CallAsync<T>(string method, object? body = null, JsonSerializerOptions? options = null);
}