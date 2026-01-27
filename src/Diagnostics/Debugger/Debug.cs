namespace TgCore.Diagnostics.Debugger;

public static class Debug
{
    public static IConsoleDebug Console;

    static Debug()
    {
        Console = new ConsoleDebug();
    }

    public static void Init(IConsoleDebug console)
    {
        Console = console;
    }
}