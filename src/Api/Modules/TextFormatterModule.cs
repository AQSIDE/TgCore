using TgCore.Diagnostics.Debugger;

namespace TgCore.Api.Modules;

public class TextFormatterModule : ITextFormatterModule
{
    private readonly HashSet<char> _md2Tags = new()
        { '_', '*', '~', '`' };

    private readonly HashSet<char> _md2TagsLeftPair = new()
        { '[', '(', '{' };
    
    private readonly HashSet<char> _md2TagsRightPair = new()
        { ']', ')', '}' };

    private readonly HashSet<char> _md2AlwaysEscapeTags = new()
        { '_', '*', '[', ']', '(', ')', '~', '`', '>', '#', '+', '-', '=', '|', '{', '}', '.', '!', '?' };

    private readonly HashSet<char> _mdTags = new()
        { '_', '*', '~', '`' };

    public bool UseLogging { get; set; } = true;

    public string Process(string text, ParseMode mode)
    {
        if (mode == ParseMode.None) return text;

        if (mode == ParseMode.Markdown)
            text = ProcessMarkdown(text, mode);
        else if (mode == ParseMode.MarkdownV2)
            text = ProcessMarkdownV2Safe(text, mode);

        return text;
    }
    
    private string ProcessMarkdownV2Safe(string text, ParseMode mode)
    {
        var stack = new Stack<int>();
        var sb = new StringBuilder(text);
        
        int unclosedTags = 0;
        for (var i = 0; i < sb.Length; i++)
        {
            var c = sb[i];
            if (_md2Tags.Contains(c) && !IsEscaped(i, text))
            {
                if (stack.Count > 0 && sb[stack.Peek()] == c)
                {
                    stack.Pop();
                    unclosedTags--;
                }
                else
                {
                    stack.Push(i);
                    unclosedTags++;
                }
            }
            else if ( _md2AlwaysEscapeTags.Contains(c) && !IsEscaped(i, text))
            {
                stack.Push(i);
            }
        }

        if (UseLogging)
            LogError(text, unclosedTags, mode);
        
        while (stack.Count > 0) 
            sb.Insert(stack.Pop(), '\\');
        
        return sb.ToString();
    }

    private string ProcessMarkdownV2(string text, ParseMode mode)
    {
        var stack = new Stack<(char symbol, int index)>();
        
        var sb = new StringBuilder(text);

        int unclosedTags = 0;
        for (int i = 0; i < sb.Length; i++)
        {
            var c = sb[i];
            
            if (_md2Tags.Contains(c) && !IsEscaped(i, text))
            {
                if (stack.Count > 0 && stack.Peek().symbol == c)
                    stack.Pop();
                else
                    stack.Push((c, i));
            }
            else if (_md2TagsLeftPair.Contains(c) && !IsEscaped(i, text))
            {
                stack.Push((c, i));
            }
            else if (_md2TagsRightPair.Contains(c) && !IsEscaped(i, text))
            {
                var tempStack = new Stack<(char symbol, int index)>();
                bool matched = false;
            
                while (stack.Count > 0)
                {
                    var top = stack.Pop();
                    if (IsMatching(top.symbol, c))
                    {
                        matched = true;
                        break;
                    }
                    else
                    {
                        tempStack.Push(top);
                    }
                }
                
                while (tempStack.Count > 0)
                    stack.Push(tempStack.Pop());
            
                if (!matched)
                    stack.Push((c, i));
            }
            else if (_md2AlwaysEscapeTags.Contains(c) && !IsEscaped(i, text))
            {
                stack.Push((c, i));
            }
        }

        if (UseLogging)
            LogError(text, unclosedTags, mode);

        while (stack.Count > 0)
            sb.Insert(stack.Pop().index, '\\');

        return sb.ToString();
    }
    
    private string ProcessMarkdown(string text, ParseMode mode)
    {
        var stack = new Stack<int>();
        
        var sb = new StringBuilder(text);

        int unclosedTags = 0;
        for (var i = 0; i < sb.Length; i++)
        {
            var c = sb[i];
            if (_mdTags.Contains(c))
            {
                if (stack.Count > 0 && sb[stack.Peek()] == c)
                {
                    stack.Pop();
                    unclosedTags--;
                }
                else
                {
                    stack.Push(i);
                    unclosedTags++;
                }
            }
        }

        if (UseLogging)
            LogError(text, unclosedTags, mode);

        while (stack.Count > 0)
            sb.Insert(stack.Pop(), '\\');

        return sb.ToString();
    }

    private bool IsEscaped(int index, string text)
    {
        if (index <= 0 || index >= text.Length)
            return false;

        int count = 0;
        for (var i = index - 1; i >= 0; i--)
        {
            var c = text[i];
            if (c == '\\') count++;
            else break;
        }

        return count % 2 == 1;
    }
    
    bool IsMatching(char left, char right)
        => (left == '[' && right == ']') ||
           (left == '(' && right == ')') ||
           (left == '{' && right == '}');

    private void LogError(string text, int count, ParseMode mode)
    {
        if (count <= 0) return;

        text = text.Length > 50 ? text.Substring(0, 50) + "..." : text;
        Debug.Console.LogWarning($"In the text: \"{text}\", {count} unclosed tags were found. Parse mode: {mode}");
    }
}