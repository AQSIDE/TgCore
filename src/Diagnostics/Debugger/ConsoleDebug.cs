namespace TgCore.Diagnostics.Debugger;

public class ConsoleDebug : IConsoleDebug
{
    private readonly object _lock = new();
    public bool UseFullDate { get; set; }
    
    public void Log(string text, LogOptions? options = null)
    {
        WriteToConsole(text, DebugType.Debug, options);
    }
    
    public void Log(DebugType type, string text, LogOptions? options = null)
    {
        WriteToConsole(text, type, options);
    }

    public void LogInfo(string text, LogOptions? options = null)
    {
        WriteToConsole(text, DebugType.Info, options);
    }

    public void LogWarning(string text, LogOptions? options = null)
    {
        WriteToConsole(text, DebugType.Warning, options);
    }

    public void LogError(string text, LogOptions? options = null)
    {
        WriteToConsole(text, DebugType.Error, options);
    }

    public void LogFatal(string text, LogOptions? options = null)
    {
        WriteToConsole(text, DebugType.Fatal, options);
    }
    
    private void WriteToConsole(string text, DebugType type, LogOptions? options)
    {
        lock (_lock)
        {
            var originalColor = Console.ForegroundColor;

            try
            {
                bool useFullDate = options?.UseFullDate ?? UseFullDate;
                
                
                Console.ForegroundColor = options?.Color ?? GetConsoleColor(type);
                Console.WriteLine(DebugHelper.GetLine(text, options?.Category, type, useFullDate, options?.ShowLevel ?? false));
            }
            finally
            {
                Console.ForegroundColor = originalColor;
            }
        }
    }

    private ConsoleColor GetConsoleColor(DebugType type)
    {
        return type switch
        {
            DebugType.Error => ConsoleColor.Red,
            DebugType.Warning => ConsoleColor.Yellow,
            DebugType.Info => ConsoleColor.Green,
            DebugType.Fatal => ConsoleColor.DarkRed,
            _ => ConsoleColor.White
        };
    }
}