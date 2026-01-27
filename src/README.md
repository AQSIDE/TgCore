# TgCore

**A flexible .NET engine for creating Telegram bots**  
The library helps quickly and structuredly build bots in C#, abstracting the routine work with the Telegram API and providing basic infrastructure.

**Installation:**

```bash
  dotnet add package TgCore
```

**Contacts & Links**
- [Nuget package](https://www.nuget.org/packages/TgCore) — install TgCore
- [Telegram](https://t.me/takeluv) - Contact me
- [Telegram](https://t.me/tgcore_chat) - TgCore community chat
- [Github Repo](https://github.com/AQSIDE/TgCore) — source code and examples
- [Support the project](https://www.donationalerts.com/r/aqside) — donations

**Limitations and development**

The library does not cover the entire Telegram Bot API.
However, the existing functionality is sufficient for creating high-quality and functional bots.

**.NET Requirements** 
- .NET 8.0 or higher
- Works on Windows, Linux, and macOS
- Using the latest stable .NET versions is recommended for better performance and compatibility

Minimum working bot

```csharp
using TgCore;

// 1. Create a bot instance
var bot = TelegramBot.Create(new TelegramClient("YOUR_BOT_TOKEN")).Build();

// 2. Create handlers
async Task UpdateHandler(Update update)
{
    if (update.Type == UpdateType.Message && update.Text != null)
    {
        await bot.Requests.SendText(update.GetFrom!.Id, $"You said: {update.Text}");
    }
}

async Task ErrorHandler(Exception exception)
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

// 6. Restart (if needed)
await bot.Restart();
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

        // Other update types...
    }
}
```

**ErrorHandler**
Handles all API errors:

```csharp
async Task ErrorHandler(Exception exception)
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
await bot.Requests.SendText(
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
await bot.Requests.SendMedia(
    chatId: chatId,
    file: InputFile.FromUrl(InputFileType.Photo, "https://example.com/photo.jpg"),
    caption: "My photo 📸",
    keyboard: InlineKeyboard.Create()
        .Row(InlineButton.CreateData("👍", "like"), InlineButton.CreateData("👎", "dislike"))
        .Build()
);        


// Direct API requests
var result = await bot.Requests.SendRequest<Message>(
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

**Configuration**

```csharp
        var client = new TelegramClient("YOUR_BOT_TOKEN"); // Telegram client (basic implementation)
        
        bot = TelegramBot
            .Create(client) // start fluent api
            .UseUpdateReceiver(new LongPollingReceiver(client, // all updates come here
                new [] { UpdateType.Message , UpdateType.CallbackQuery}, // allowed updates
                limit:100, // max update limit
                timeout:30, // long polling timeout
                startOffset:0)) // update id offset
            .UseLoopRunner(new BotLoopRunner()) // boot loop runner
            .UseDefaultParseMode(ParseMode.MarkdownV2) // default parse mode if parameter null
            .UseLifetime() // Message deletion time (Lifetime) configuration
            .UseRateLimit(new RateLimitModule( // Request limiting (Rate Limiting) configuration
                requestsPerSecond:20,  // 20 requests per second
                maxBurstSize:25)) // Maximum burst
            .UseTemporaryMessageLimiter(new TemporaryMessageLimiterModule( // Temporary message limiter
                    maxMessageLimit:3, // Maximum of 3 temporary messages
                    mode:TemporaryLimiterMode.Reject)) // Mode when the limit is exceeded
            .Build();
        
        // Subscribe to Lifetime events
        bot.Options.Lifetime.OnAdd = OnAdd;
        bot.Options.Lifetime.OnDelete = OnDelete;
);
```

**Advanced architecture**

```csharp
public class Program
{
    private static ContextFactory _contextFactory = null!;
    private static BuildFactory _buildFactory = null!;
    private static RouterManager _routerManager = null!;

    private static TelegramBot _bot = null!;

    private static async Task Main()
    {
        _bot = TelegramBot
            .Create(new TelegramClient("YOUR_BOT_TOKEN"))
            .UseDefaultParseMode(ParseMode.MarkdownV2)
            .UseLifetime()
            .UseRateLimit()
            .UseTemporaryMessageLimiter()
            .Build();

        _bot.AddUpdateHandler(UpdateHandler)
            .AddErrorHandler(ErrorHandler);

        _buildFactory = new BuildFactory(_bot);
        _contextFactory = new ContextFactory(_bot);

        _routerManager = _buildFactory.BuildRoute();

        await _bot.Run();
    }

    private static async Task UpdateHandler(Update update)
    {
        var ctx = _contextFactory.CreateContext(update);
        if (ctx == null) return;

        await _routerManager.Route(ctx);
    }

    private static async Task ErrorHandler(Exception ex)
    {
        Debug.LogError(ex.ToString());
    }
}
```

## Bot Loop
Bot Loop is an infinite loop that triggers at a specified interval in milliseconds.
It is used to simplify prototyping and avoid creating Task.Run manually.

**IBotLoop interface**

```csharp
public interface IBotLoop
{
    // Interval between ticks in milliseconds
    int IntervalMs { get; }
    
    // Method called on each tick
    Task OnTick();
}
```

**Implementation example**

```csharp
public class YourLoop : IBotLoop
{
    // Interval between ticks in milliseconds.
    public int IntervalMs { get; set; }

    // Initializes a new instance of YourLoop with the specified interval.
    public YourLoop(int intervalMs)
    {
        IntervalMs = intervalMs;
    }
    
    // Method called on each tick.
    public async Task OnTick()
    {
        // Code executed on each tick
    }
}

// Adding the loop to the bot
bot.AddLoop(new YourLoop(100));
```

**Basic usage**

```csharp
// Class for delayed and recurring tasks.
// No need to create manually, it is already included in TelegramBot:
// bot.MainLoop
public class BotTaskLoop : IBotLoop
{
    // Implementation details are handled internally
}
    
// Example: Adding a delayed task using Func<Task>
bot.MainLoop.AddTask(
    DateTime.Now.AddSeconds(5), // Executes after 5 seconds
    Execute                     // The function to execute
);

// Example: Adding a repeating task using Func<Task>
bot.MainLoop.AddRepeatingTask(
    TimeSpan.FromSeconds(5),    // Interval between executions
    Execute,                    // The function to execute
    DateTime.Now.AddSeconds(5)  // Start time for the first execution
);
```
## Modules
Modules are built-in features that simplify working with the API.

**ILifetimeModule**
Manages message lifetime and allows automatic deletion after a set time.

```csharp
public interface ILifetimeModule
{
    // Invoked when a message is added.
    // Parameters: long chatId/userId, long messageId
    Func<long, long, Task>? OnAdd { get; set; }
    
    // Invoked when a message is deleted.
    // Parameters: long chatId/userId, long messageId
    Func<long, long, Task>? OnDelete { get; set; }
    
    // Marks a message for automatic deletion after the specified lifetime.
    public Task Set(long chatId, long messageId, TimeSpan lifetime);
    
    // Removes the deletion mark from a message.
    public Task<bool> Remove(long chatId, long messageId);
    
    // Deletes a message immediately.
    public Task<bool> Delete(long chatId, long messageId);
    
    // Clears all deletion marks for a specific chatId/userId.
    public void ClearMessages(long chatId);
}

// Activating the module
bot.Options.Lifetime = new LifetimeModule(_bot, _bot.MainLoop)
{
    // Enable module logging
    UseLogging = true
};

// Usage example
await bot.Requests.SendText(
    user.Id,
    $"⏳ <b>{user.Username}</b>, {TextFormatter.WaitTime(activity.timeLeft.TotalSeconds)}", 
    replyId:context.MessageId,
    lifeTime:TimeSpan.FromSeconds(5)); // Message will be deleted after 5 seconds
```

**IRateLimitModule**
Manages request rate limits automatically to avoid exceeding Telegram API limits.

```csharp
public interface IRateLimitModule
{
    // Waits until the next request can be sent according to the rate limit.
    ValueTask WaitAsync(CancellationToken ct = default);
}

// Activating the module
bot.Options.RateLimit = new RateLimitModule(
            requestsPerSecond:20,   // Maximum 20 requests per second
            maxBurstSize:25);       // Maximum burst size
```

**ITemporaryMessageLimiterModule**
Limits temporary messages in chat, used with ILifetimeModule to prevent spam.

```csharp
public interface ITemporaryMessageLimiterModule
{
    // Maximum number of temporary messages allowed per chat
    int MaxMessageLimit { get; set; }

    // Checks if a new message can be sent in the chat
    Task<bool> CanSend(long chatId);
    
    // Registers a sent temporary message
    Task RegisterMessage(long chatId, long messageId);
    
    // Unregisters a temporary message
    Task UnregisterMessage(long chatId, long messageId);
}

// Activating the module
_bot.Options.TemporaryMessageLimiter = new TemporaryMessageLimiterModule(
            maxMessageLimit:3,                         // Maximum of 3 temporary messages
            mode:TemporaryLimiterMode.ReplaceOldest,  // Mode when the limit is exceeded
            lifetimeModule:_bot.Options.Lifetime)     // Uses ILifetimeModule
        {
            // Enable module logging
            UseLogging = true,
        };
       
// Operation modes
public enum TemporaryLimiterMode
{
    Reject,         // Limit exceeded: the new message will not be sent
    ReplaceOldest,  // Deletes the oldest message and sends the new one
    ReplaceNewest   // Deletes the newest message, keeping the old one
}
```