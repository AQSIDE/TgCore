using TgCore.Api.Bot;
using TgCore.Api.Types.Poll;

namespace TestBot.Commands;

public class PollCommand : TelegramCommand
{
    public PollCommand(TelegramBot bot) : base(bot)
    {
    }

    public override string Name => "/poll";
    public override async Task ExecuteAsync(long chatId, long? messageId, string[]? args = null)
    {
        await _bot.Requests.SendPoll(chatId, "2 + 2 = ?", new []
        {
            InputPollOption.Create("5"), 
            InputPollOption.Create("10"), 
            InputPollOption.Create("4"), 
        }, allowsMultipleAnswers:true);
    }
}