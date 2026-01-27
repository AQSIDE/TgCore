using TgCore.Api.Bot;
using TgCore.Api.Enums;
using TgCore.Api.Types.Poll;

namespace TestBot.Commands;

public class QuizCommand : TelegramCommand
{
    public QuizCommand(TelegramBot bot) : base(bot)
    {
    }

    public override string Name => "/quiz";

    public override async Task ExecuteAsync(long chatId, long? messageId, string[]? args = null)
    {
        await _bot.Requests.SendPoll(chatId, "2 + 2 = ?", new[]
            {
                InputPollOption.Create("5"),
                InputPollOption.Create("10"),
                InputPollOption.Create("4"),
            },
            type: PollType.Quiz,
            isAnonymous: true,
            allowsMultipleAnswers: false,
            correctOptionId: 2,
            explanation: "Yes! 2 + 2 = 4.");
    }
}