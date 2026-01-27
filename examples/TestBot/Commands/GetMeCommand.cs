using TgCore.Api.Bot;

namespace TestBot.Commands;

public class GetMeCommand : TelegramCommand
{
    public GetMeCommand(TelegramBot bot) : base(bot)
    {
    }

    public override string Name => "/get_me";
    public override async Task ExecuteAsync(long chatId, long? messageId, string[]? args = null)
    {
        var user = await _bot.Requests.GetMe();
        
        await _bot.Requests.SendText(
            chatId,
            $"👋 Hello! I am <b>{user?.FirstName}</b> (@{user?.Username})",
            replyId: messageId
        );
    }
}