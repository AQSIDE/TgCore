# TgCore

**A flexible .NET engine for creating Telegram bots**  
The library helps quickly and structuredly build bots in C#, abstracting the routine work with the Telegram API and providing basic infrastructure.

Minimum working bot

```csharp
using TgCore;

// 1. Create a bot instance
var bot = new TelegramBot(new BotOptions("YOUR_BOT_TOKEN"));

// 2. Create handlers
async Task UpdateHandler(Update update)
{
    if (update.Type == UpdateType.Message && update.Text != null)
    {
        await bot.Message.SendText(update.GetFrom!.Id, $"You said: {update.Text}");
    }
}

async Task ErrorHandler(Exception exception, Update? update)
{
    Console.WriteLine($"Error: {exception.Message}");
}

// 3. Register handlers
bot.AddUpdateHandler(UpdateHandler);
bot.AddErrorHandler(ErrorHandler);

// 4. Start the bot
await bot.Run();

// 5. Stop (if needed)
await bot.Stop();
```

## Basic concepts

**UpdateHandler**
Handles all user actions:

```csharp
async Task UpdateHandler(Update update)
{
    // Determine the event type
    switch (update.Type)
    {
        case UpdateType.Message:
            // Work with the message
            await HandleMessage(update);
            break;

        case UpdateType.CallbackQuery:
            // Handle button click
            await bot.Message.AnswerCallback(update.CallbackQuery!.Id);
            await HandleCallback(update);
            break;

        // Other event types...
    }
}
```

**ErrorHandler**
Handles all API errors:

```csharp
async Task ErrorHandler(Exception exception, Update? update)
{
    // Log the error
    Console.WriteLine($"Error: {exception}");
}
```

**Working with requests**

```csharp
// Simple text
await bot.Message.SendText(chatId, "Hello!");

// With formatting
await bot.Message.SendText(
    chatId: chatId,
    text: "*Bold text* and `code`",
    parseMode: ParseMode.Markdown,
    keyboard: InlineKeyboard.Create()
        .Row(InlineButton.CreateUrl("Link", "https://github.com/AQSIDE"))
        .Row(InlineButton.CreateData("Button 1", "btn1"), InlineButton.CreateData("Button 2", "btn1"))
        .Build(),
    lifeTime: TimeSpan.FromSeconds(30) // Self-deletion in 30 seconds
);

// Photo with caption
await bot.Message.SendMedia(
    chatId: chatId,
    file: InputFile.FromUrl(InputFileType.Photo, "https://example.com/photo.jpg"),
    text: "My photo 📸",
    keyboard: InlineKeyboard.Create()
        .Row(InlineButton.CreateData("👍", "like"), InlineButton.CreateData("👎", "dislike"))
        .Build(),


// Direct API requests
var result = await bot.Message.SendRequest<Message>(
    method: TelegramMethods.SEND_MESSAGE,
    body: new
    {
        chat_id = chatId,
        photo = "https://example.com/photo.jpg",
        caption = "Photo via direct request",
        reply_markup = new { inline_keyboard = new[] { new[] { new { text = "Test", callback_data = "test" } } } }
    }
);

if (result.Ok)
{
    Console.WriteLine($"Message sent with ID: {result.Result!.MessageId}");
}

// Deletion
bool success = await bot.Message.DeleteMessage(chatId, messageId);
```

**Advanced architecture**

```csharp
async Task UpdateHandler(Update update)
{
    // Create context
    var context = CreateContext(update);
    if (context == null) return;

    var user = context.User;

    // Update user activity
    _activityService.UpdateUserActivity(user);

    // Answer callback to remove "loading"
    if (update.Type == UpdateType.CallbackQuery)
    {
        await bot.Message.AnswerCallback(update.CallbackQuery!.Id);
    }

    // Route the request further
    await _routerManager.Route(context);
}

private UserContext? CreateContext(Update update)
{
    if (update.GetFrom == null) return null;

    return new UserContext
    {
        UserId = update.GetFrom.Id,
        ChatId = update.GetChat?.Id ?? update.GetFrom.Id,
        Update = update,
        UserData = _userRepository.Get(update.GetFrom.Id)
    };
}
```

**Configuration**

```csharp
bot = new TelegramBot(new BotOptions(
            "YOUR_TOKEN",
            allowedUpdates: new[]
            {
                UpdateType.Message,
                UpdateType.CallbackQuery,
            },
            defaultParseMode: ParseMode.HTML)
        {
            Offset = 0,                    // From which update to start
            Timeout = 30,                  // Long polling timeout in seconds
        });

        // Message deletion time (Lifetime) configuration
        _bot.Options.Lifetime = new LifetimeOptions(new LifetimeModule(bot, bot.MainLoop));
        
        // Subscribe to Lifetime events
        bot.Options.Lifetime.Module.OnAdd = OnAdd;
        bot.Options.Lifetime.Module.OnDelete = OnDelete;

        // Request limiting (Rate Limiting) configuration
        bot.Options.RateLimit = new RateLimitOptions( new RateLimiter(
            requestsPerSecond: 20,    // 20 requests per second
            maxBurstSize: 25          // Maximum burst
        )
);
```
