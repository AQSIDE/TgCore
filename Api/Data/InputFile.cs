namespace TgCore.Api.Data;

public sealed class InputFile
{
    public InputFileType FileType { get; }
    public string? FileId { get; }
    public string? FileName { get; }
    public string? Url { get; }
    public Stream? Stream { get; }

    private InputFile(InputFileType fileType, string? fileId, string? url, Stream? stream, string? fileName)
    {
        FileType = fileType;
        FileId = fileId;
        Url = url;
        Stream = stream;
        FileName = fileName;
    }
    
    public object? GetValue()
    {
        if (!string.IsNullOrEmpty(FileId))
            return FileId!;
        if (!string.IsNullOrEmpty(Url))
            return Url!;
        if (Stream != null && !string.IsNullOrEmpty(FileName))
            return Stream;
        
        return null;
    }
    
    public static InputFile Create(
        InputFileType type,
        string? fileId = null,
        string? url = null,
        Stream? stream = null,
        string? fileName = null)
    {
        if (stream != null && string.IsNullOrEmpty(fileName))
            throw new ArgumentException("FileName must be set when using Stream");
        
        return new InputFile(type, fileId, url, stream, fileName);
    }
    
    public static InputFile FromFileId(InputFileType fileType, string fileId) 
        => new(fileType, fileId, null, null, null);

    public static InputFile FromUrl(InputFileType fileType, string url) 
        => new(fileType, null, url, null, null);

    public static InputFile FromStream(InputFileType fileType, Stream stream, string fileName) 
        => new(fileType, null, null, stream, fileName);
}