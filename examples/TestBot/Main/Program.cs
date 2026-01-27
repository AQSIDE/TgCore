using TgCore.Api.Bot;
using TgCore.Api.Clients;
using TgCore.Api.Enums;
using TgCore.Api.Runtime;
using TgCore.Api.Types;
using TgCore.Diagnostics.Debugger;

namespace TestBot.Main;

class Program
{
    private static readonly string _token = "YOUR_BOT_TOKEN";
    private static readonly UpdateType[] _allowedUpdates = Enum.GetValues<UpdateType>();
    
    private static CommandFactory _commandFactory;
    private static TelegramBot _bot;

    static async Task Main()
    {
        var client = new TelegramClient(_token);
        
        _bot = TelegramBot
            .Create(client)
            .UseDefaultParseMode(ParseMode.HTML)
            .UseUpdateReceiver(new LongPollingReceiver(client, _allowedUpdates))
            .UseLifetime()
            .UseRateLimit()
            .UseTemporaryMessageLimiter()
            .Build();

        _bot.AddUpdateHandler(UpdateHandler)
            .AddErrorHandler(ErrorHandler);

        _commandFactory = new CommandFactory(_bot);

        await _bot.Run();
    }

    static async Task UpdateHandler(Update update)
    {
        var chatId = update.FromOrChatId;
        var messageId = update.MessageId;
        
        Debug.Console.Log($"updateType: {update.Type.ToString()}", new LogOptions { UseFullDate = true});
        
        if (update.Type == UpdateType.Message)
        {
            var text = update.Message!.Text!;

            if (_commandFactory.GetCommand(text, out var command))
            {
                await command!.ExecuteAsync(chatId!.Value, messageId!.Value);
            }
            else
            {
                await _bot.Requests.SendText(chatId!.Value, "Unsupported command", replyId: messageId);
            }
        }
    }

    static async Task ErrorHandler(Exception ex)
    {
        Debug.Console.LogError(ex.ToString());
    }
}