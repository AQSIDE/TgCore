namespace TgCore.Sdk.Text;

public class BotTextFormatter
{
    public static string Substring(string text, int maxLength, string addToEnd = "...")
    {
        if (string.IsNullOrEmpty(text)) 
            return text ?? string.Empty;
        
        if  (text.Length <= maxLength) 
            return text;
        
        int actualMaxLength = maxLength - addToEnd.Length;
        
        if (actualMaxLength <= 0)
            return addToEnd.Length > maxLength 
                ? addToEnd.Substring(0, maxLength)
                : addToEnd;
        
        return text.Substring(0, actualMaxLength) + addToEnd;
    }
}