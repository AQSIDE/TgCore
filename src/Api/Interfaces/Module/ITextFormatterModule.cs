namespace TgCore.Api.Interfaces.Module;

public interface ITextFormatterModule
{
    string Process(string text, ParseMode mode);
}