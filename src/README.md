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
var bot = TelegramBot.Default("YOUR_BOT_TOKEN");

// 2. Create handlers
async Task UpdateHandler(Update update, CancellationToken ct)
{
    if (update.Type == UpdateType.Message && update.Text != null)
    {
        await bot.Requests.SendText(update.GetFrom!.Id, $"You said: {update.Text}", ct:ct);
    }
}

async Task ErrorHandler(Exception exception, CancellationToken ct)
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
async Task UpdateHandler(Update update, CancellationToken ct)
{
    // Determine the event type
    switch (update.Type)
    {
        case UpdateType.Message:
            // Work with the message
            await HandleMessage(update, ct);
            break;

        case UpdateType.CallbackQuery:
            // Handle button click
            await bot.Message.AnswerCallbackQuery(update.CallbackQuery!.Id, ct:ct);
            await HandleCallback(update, ct);
            break;

        // Other update types...
    }
}
```

**ErrorHandler**
Handles all API errors:

```csharp
async Task ErrorHandler(Exception exception, CancellationToken ct)
{
    // Log the error
    Console.WriteLine($"Error: {exception}");
}
```

**Working with requests**

```csharp
// Simple text
await bot.Requests.SendText(chatId, "Hello!");

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
    }, 
    ct:ct
);

if (result.Ok)
{
    Console.WriteLine($"Message sent with ID: {result.Result!.MessageId}");
}

// Deletion
bool success = await bot.Requests.DeleteMessage(chatId, messageId);
```

**Configuration**

```csharp
var client = new TelegramClient("YOUR_BOT_TOKEN"); // Telegram client (base implementation)

bot = TelegramBot
    .Create(client) // Start bot builder
    .UseUpdateReceiver(
        new LongPollingReceiver(
            client,                                     // Source of incoming updates
            new[] { UpdateType.Message, UpdateType.CallbackQuery }, // Allowed update types
            limit: 100,                                 // Maximum updates per request
            timeout: 30,                                // Long polling timeout (seconds)
            startOffset: 0                              // Update ID offset
        )
    )
    .UseLoopRunner(new BotLoopRunner())                // Start bot loop runner
    .UseTelemetry()                                    // Enable automatic telemetry
    .UseDefaultParseMode(ParseMode.MarkdownV2)         // Default parse mode if not specified
    .Build();

bot.Modules                                            // Configure modules
    .UseMessageLifetime()                              // Message lifetime (auto-delete) module
    .UseRateLimit(new RateLimitModule(
        maxTokens: 20,                                 // Max 20 requests per second
        maxBurst: 3,                                   // Allowed burst size
        interval: TimeSpan.FromSeconds(1)
    ))
    .UseTextFormatter()                                // Text formatter for parse_mode (beta)
    .UseTemporaryMessageLimiter(
        new TemporaryMessageLimiterModule(             // Temporary message limiter
            maxMessageLimit: 3,                         // Maximum 3 temporary messages
            mode: TemporaryLimiterMode.Reject           // Behavior when limit is exceeded
        )
    )
    .Apply();

// Subscribe to message lifetime events
bot.Options.Lifetime.OnAdd = OnAdd;
bot.Options.Lifetime.OnDelete = OnDelete;
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
        var client = new TelegramClient("YOUR_BOT_TOKEN");
        
        _bot = TelegramBot
            .Create(client)
            .UseDefaultParseMode(ParseMode.MarkdownV2)
            .UseTelemetry(new TelemetryConfig(interval:TimeSpan.FromMinutes(10)))
            .UseUpdateReceiver(new LongPollingReceiver(client, _allowedUpdates))
            .Build();
        
        _bot.Modules
            .UseMessageLifetime()
            .UseRateLimit()
            .UseTextFormatter() // (beta)
            .UseTemporaryMessageLimiter()
            .Apply();

        _bot.AddUpdateHandler(UpdateHandler)
            .AddErrorHandler(ErrorHandler);

        _buildFactory = new BuildFactory(_bot);
        _contextFactory = new ContextFactory(_bot);

        _routerManager = _buildFactory.BuildRoute();

        await _bot.Run();
    }

    private static async Task UpdateHandler(Update update, CancellationToken ct)
    {
        var ctx = _contextFactory.CreateContext(update);
        if (ctx == null) return;

        await _routerManager.Route(ctx, ct);
    }

    private static async Task ErrorHandler(Exception exception, CancellationToken ct)
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
## Telemetry
**Telemetry is a system that collects runtime data about everything that is happening inside your bot.
It is designed to help you understand bot behavior, performance, errors, and usage patterns in real time.**

```csharp
// Fluent API usage
bot = TelegramBot
    .Create(client)
    .UseTelemetry() // Enable telemetry here
    .Build();

// OR via options 

var options = new BotOptions(client) 
{
    InitialUseTelemetry = true
};
```

**Telemetry uses a configuration object to control memory usage, compatibility, and behavior.**
All limits exist to prevent uncontrolled memory growth.

```csharp
public TelemetryConfig(
    bool useAutoLog = true,        // Enable automatic logging to console
    TimeSpan? interval = null,     // Interval between snapshot reports
    int maxRequests = 30,          // Maximum stored HTTP requests
    int maxUpdates = 30,           // Maximum stored updates
    int maxUpdateHandlers = 30,    // Maximum stored UpdateHandler executions
    int maxErrorHandlers = 30,     // Maximum stored ErrorHandler executions
    int maxUsers = 30,             // Maximum stored users
    int maxChats = 30,             // Maximum stored chats
    int maxErrors = 30,            // Maximum stored errors
    int maxInteractionContext = 10,// Stored interaction context per user/chat
    bool allowPrivateChat = false  // Include private chats in chat statistics
)
```

**What Telemetry Stores**
Each telemetry report is represented by a snapshot, which contains aggregated and detailed runtime data.

```csharp
public sealed class TelemetrySnapshotDto
{
    // Unique snapshot identifier
    public long Id { get; init; }
    
    // Snapshot creation time
    public DateTime Timestamp { get; init; }

    // Updates received during the current period
    public long PeriodUpdates { get; init; }
    
    // Updates received since bot start
    public long TotalUpdates { get; init; }

    // HTTP request counters
    public long PeriodRequests { get; init; }
    public long TotalRequests { get; init; }
    
    public long PeriodSuccessfulRequests { get; init; }
    public long PeriodFailedRequests { get; init; }
    public long TotalSuccessfulRequests { get; init; }
    public long TotalFailedRequests { get; init; }

    // Error counters
    public long PeriodErrors { get; init; }
    public long TotalErrors { get; init; }

    // Unique users
    public long PeriodUniqueUsers { get; init; }
    public long TotalUniqueUsers { get; init; }
    
    // Unique chats
    public long PeriodUniqueChats { get; init; }
    public long TotalUniqueChats { get; init; }

    // Derived metrics
    public double UpdatesPerUser { get; init; }
    public double RequestsPerUser { get; init; }
    
    public double UpdatesPerChat { get; init; }
    public double RequestsPerChat { get; init; }

    // Latency statistics (avg / min / max)
    public TelemetryLatencyStats UpdateHandlerLatency { get; init; }
    public TelemetryLatencyStats ErrorHandlerLatency { get; init; }
    public TelemetryLatencyStats HTTPLatency { get; init; }

    // Detailed runtime data
    public List<TelemetryUserDto> Users { get; init; }
    public List<TelemetryChatDto> Chats { get; init; }
    public List<TelemetryUpdateDto> Updates { get; init; }
    public List<TelemetryRequestDto> Requests { get; init; }
    public List<TelemetryErrorDto> Errors { get; init; }

    // Error classification
    public List<ApiErrorStats> ApiErrors { get; init; }
    public List<LocalErrorStats> LocalErrors { get; init; }
}
```

**Using Telemetry at Runtime**

```csharp
bot.Telemetry.Config;               // Access current telemetry configuration
bot.Telemetry.Enabled;              // Enable / disable telemetry collection
bot.Telemetry.LastAutoLogMessage;   // Last auto-log message
bot.Telemetry.LastSnapshot;         // Get last snapshot (can be null)
bot.Telemetry.GetSnapshot();        // Force snapshot creation
bot.Telemetry.Update(s => s.ClearUniqueCache()); // Apply snapshot update

// Report Subscription
// You can subscribe to telemetry reports and handle them manually.

bot.Telemetry.OnReport = OnReport;
bot.Telemetry.Config.UseAutoLog = false; // Recommended when using OnReport

// Called asynchronously on each report
// lastSnapshot can be null (first report)
async Task OnReport(
    TelemetrySnapshotDto snapshot,
    TelemetrySnapshotDto? lastSnapshot)
{
    // Custom processing (logging, metrics export, alerts, etc.)
}
```

**Important Notes.** Telemetry permanently stores unique user and chat IDs to provide accurate lifetime statistics. This cache is not cleared automatically.
To manually reset unique user/chat counters, use:

```csharp
bot.Telemetry.Update(s => s.ClearUniqueCache());
```

## Modules
Modules are built-in features that simplify working with the API.

**IMessageLifetimeModule**
Manages message lifetime and allows automatic deletion after a set time.

```csharp
public interface IMessageLifetimeModule
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
bot.Options.Lifetime = new MessageLifetimeModule(_bot, _bot.MainLoop)
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
            maxTokens:20,                       // Maximum 20 requests per second
            maxBurst:3,                         // Maximum burst size
            interval: TimeSpan.FromSeconds(1)); // interval
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
            maxMessageLimit:3,                         // Maximum of 3 temporary messages for user/chat
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

**ITextFormatterModule**
Automatically formats outgoing message text. This module is applied when text is sent as part of a request.

**Why this exists.**
Telegram will throw a BadRequest error if the message contains invalid or unclosed Markdown tags.
TextFormatterModule automatically fixes such cases, making message sending safer and more predictable.

```csharp
public interface ITextFormatterModule
{
    string Process(string text, ParseMode mode);
}
```

**Default Implementation**

```csharp
// Formats text if it contains unclosed tags.
// Applied to Markdown and MarkdownV2 to prevent BadRequest errors.
public class TextFormatterModule : ITextFormatterModule

public string Process(string text, ParseMode mode)
{
    if (mode == ParseMode.None) return text;

    if (mode == ParseMode.Markdown)
        text = ProcessMarkdown(text, mode);
    else if (mode == ParseMode.MarkdownV2)
        text = ProcessMarkdownV2(text, mode);

    return text;
}
```