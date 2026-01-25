namespace TgCore.Sdk.Validation;
    
public class BotTextFormatter
{
    public static string Truncate(string text, int maxLength, string suffix = "...")
    {
        if (string.IsNullOrEmpty(text)) 
            return text ?? string.Empty;
        
        if  (text.Length <= maxLength) 
            return text;
        
        int actualMaxLength = maxLength - suffix.Length;
        
        if (actualMaxLength <= 0)
            return suffix.Length > maxLength 
                ? suffix.Substring(0, maxLength)
                : suffix;
        
        return text.Substring(0, actualMaxLength) + suffix;
    }

    public static string Join(IEnumerable<string> values, string separator)
    {
        return string.Join(separator, values);
    }
}

