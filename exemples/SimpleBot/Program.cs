using TgCore.Api.Bot;
using TgCore.Api.Data;
using TgCore.Api.Enums;
using TgCore.Api.Types;
using TgCore.Diagnostics.Debugger;

class Program
{
    private static TelegramBot _bot;
    
    static async Task Main()
    {
        _bot = new TelegramBot(new BotOptions("YOUR_BOT_TOKEN"));
        
        _bot.AddUpdateHandler(UpdateHandler);
        _bot.AddErrorHandler(ErrorHandler);

        await _bot.Run();
    }

    static async Task UpdateHandler(Update update)
    {
        switch (update.Type)
        {
            case UpdateType.Message:
                await _bot.Message.SendText(update.FromId!.Value, update.Message!.Text!);
                break;
        }
    }

    static async Task ErrorHandler(Exception ex)
    {
        Debug.LogError(ex.ToString());
    }
}