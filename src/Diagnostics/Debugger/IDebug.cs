namespace TgCore.Diagnostics.Debugger;

public interface IDebug
{
    void Log(string text, LogOptions? options = null);
    void Log(DebugType type, string text, LogOptions? options = null);
    void LogInfo(string text, LogOptions? options = null);
    void LogWarning(string text, LogOptions? options = null);
    void LogError(string text, LogOptions? options = null);
    void LogFatal(string text, LogOptions? options = null);
}