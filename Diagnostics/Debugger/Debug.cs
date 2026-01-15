namespace TgCore.Diagnostics.Debugger;

public static class Debug
{
    private static readonly Lock Lock = new();
    
    public static bool UseFullDate { get; set; } = false;

    public static void Log(string text, string? relation = null)
    {
        WriteToConsole(text, relation, DebugType.Debug);
    }

    public static void Log(DebugType type, string text, string? relation = null)
    {
        WriteToConsole(text, relation, type);
    }

    public static void LogWarning(string text, string? relation = null)
    {
        WriteToConsole(text, relation, DebugType.Warning);
    }

    public static void LogError(string text, string? relation = null)
    {
        WriteToConsole(text, relation, DebugType.Error);
    }

    public static void LogInfo(string text, string? relation = null)
    {
        WriteToConsole(text, relation, DebugType.Info);
    }

    private static void WriteToConsole(string text, string? relation, DebugType type)
    {
        lock (Lock)
        {
            var originalColor = Console.ForegroundColor;

            try
            {
                var now = DateTime.Now;
                var dateString = UseFullDate
                    ? now.ToString("dd.MM.yyyy HH:mm:ss")
                    : now.ToString("HH:mm:ss"); 
                
                string extraInfo = string.IsNullOrEmpty(relation) ? " " : $", [{relation}] ";
                Console.ForegroundColor = GetConsoleColor(type);
                Console.WriteLine($"[{dateString}]{extraInfo}{text}");
            }
            finally
            {
                Console.ForegroundColor = originalColor;
            }
        }
    }

    private static ConsoleColor GetConsoleColor(DebugType type)
    {
        return type switch
        {
            DebugType.Error => ConsoleColor.Red,
            DebugType.Warning => ConsoleColor.Yellow,
            DebugType.Info => ConsoleColor.Green,
            _ => ConsoleColor.White
        };
    }
}