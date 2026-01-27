namespace TgCore.Diagnostics.Debugger;

public static class DebugHelper
{
    public static string GetLine(string text, string? category, DebugType type, bool useFullDate, bool showLevel)
    {
        var now = DateTime.Now;

        var dateString = useFullDate
            ? now.ToString("dd.MM.yyyy HH:mm:ss")
            : now.ToString("HH:mm:ss");

        var sb = new StringBuilder();

        sb.Append('[').Append(dateString).Append(']');

        if (showLevel)
            sb.Append(" [").Append(type).Append(']');

        if (!string.IsNullOrEmpty(category))
            sb.Append(" [").Append(category).Append(']');

        sb.Append(' ').Append(text);

        return sb.ToString();
    }
}