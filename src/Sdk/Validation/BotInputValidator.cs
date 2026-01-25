namespace TgCore.Sdk.Validation;

public class BotInputValidator
{
    public static bool ValidateNumber(string text, int minValue, int maxValue, out int result)
    {
        result = 0;
        
        if (string.IsNullOrEmpty(text)) return false;
        
        if (!int.TryParse(text.Trim(), out var number))
            return false;
        
        if (number < minValue || number > maxValue)
            return false;
        
        result = number;
        return true;
    }
    
    public static bool ValidateText(string text, out string result, uint minLength = 0, uint maxLength = 9999)
    {
        result = text?.Trim() ?? string.Empty;
        
        if (string.IsNullOrEmpty(text)) return false;
        
        if (result.Length < minLength || result.Length > maxLength) 
            return false;
        
        return true;
    }

    public static T? SelectByIndex<T>(T[] array, string text, int offset = 0)
    {
        if (!int.TryParse(text, out int index))
            return default;
    
        index -= offset;
    
        if (index < 0 || index >= array.Length)
            return default;
    
        return array[index];
    }
    
    public static T? SelectByIndex<T>(T[] array, int index)
    {
        if (index < 0 || index >= array.Length)
            return default;
        
        return array[index];
    }
}