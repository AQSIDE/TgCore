namespace TgCore.Utils;

public class PathHelper
{
    private static string _projectRoot =
        Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "../../../../../../"));

    public static string ProjectRoot
    {
        get => _projectRoot;
        set => _projectRoot = value;
    }

    public static string GetProjectRoot() => _projectRoot;

    public static string GetLocalPath(string relativePath)
    {
        return Path.GetFullPath(Path.Combine(_projectRoot, relativePath));
    }

    public static T LoadJson<T>(string path, JsonSerializerOptions? options = null)
    {
        if (!File.Exists(path))
            throw new FileNotFoundException($"JSON file not found: {path}");

        var json = File.ReadAllText(path);
        return JsonSerializer.Deserialize<T>(json, options)
               ?? throw new JsonException($"Failed to deserialize {typeof(T).Name}");
    }
}