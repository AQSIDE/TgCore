namespace TgCore.Api.Data;

public class BotOptions
{
    public string Token { get; }
    public long Offset { get; set; }
    public int Timeout { get; set; } = 30;
    public UpdateType[] AllowedUpdates { get; set; }
    public ParseMode DefaultParseMode { get; set; }

    public BotOptions(string token, UpdateType[]? allowedUpdates = null, ParseMode defaultParseMode = ParseMode.None)
    {
        Token = token;
        AllowedUpdates = allowedUpdates ?? new [] { UpdateType.Message, UpdateType.CallbackQuery };
        DefaultParseMode = defaultParseMode;
    }
}