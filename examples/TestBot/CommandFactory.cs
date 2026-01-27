using TestBot.Commands;
using TgCore.Api.Bot;

namespace TestBot;

public class CommandFactory
{
    private readonly List<ITelegramCommand> _commands = new();

    public CommandFactory(TelegramBot bot)
    {
        _commands.Add(new StartCommand(bot));
        _commands.Add(new TextCommand(bot));
        _commands.Add(new PhotoCommand(bot));
        _commands.Add(new VideoCommand(bot));
        _commands.Add(new AudioCommand(bot));
        _commands.Add(new DocumentCommand(bot));
        _commands.Add(new AnimationCommand(bot));
        _commands.Add(new PollCommand(bot));
        _commands.Add(new QuizCommand(bot));
        _commands.Add(new GetMeCommand(bot)); 
        _commands.Add(new InlineCommand(bot)); 
        _commands.Add(new ReplyCommand(bot)); 
        _commands.Add(new MediaGroupCommand(bot)); 
        _commands.Add(new DiceCommand(bot)); 
    }

    public bool GetCommand(string commandName, out ITelegramCommand? command)
    {
        command = _commands.SingleOrDefault(x => x.Name == commandName);
        return command != null;
    }
}