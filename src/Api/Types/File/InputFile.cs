namespace TgCore.Api.Types.File;

public sealed class InputFile
{
    public InputFileType FileType { get; }
    public string? FileId { get; }
    public string? FileName { get; }
    public string? Url { get; }
    public Stream? Stream { get; }
    public bool HasSpoiler { get; }
    public bool ShowCaptionAboveMedia { get; }
    

    private InputFile(InputFileType fileType, string? fileId, string? url, Stream? stream, string? fileName, bool hasSpoiler, bool showCaptionAboveMedia)
    {
        FileType = fileType;
        FileId = fileId;
        Url = url;
        Stream = stream;
        FileName = fileName;
        HasSpoiler = hasSpoiler;
        ShowCaptionAboveMedia = showCaptionAboveMedia;
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
        string? fileName = null,
        bool hasSpoiler = false,
        bool showCaptionAboveMedia = false)
    {
        if (stream != null && string.IsNullOrEmpty(fileName))
            throw new ArgumentException("FileName must be set when using Stream");
        
        return new InputFile(type, fileId, url, stream, fileName, hasSpoiler, showCaptionAboveMedia);
    }
    
    public static InputFile FromFileId(InputFileType fileType, string fileId, bool hasSpoiler = false, bool showCaptionAboveMedia = false) 
        => new(fileType, fileId, null, null, null, hasSpoiler, showCaptionAboveMedia);

    public static InputFile FromUrl(InputFileType fileType, string url, bool hasSpoiler = false, bool showCaptionAboveMedia = false) 
        => new(fileType, null, url, null, null, hasSpoiler, showCaptionAboveMedia);

    public static InputFile FromStream(InputFileType fileType, Stream stream, string fileName, bool hasSpoiler = false, bool showCaptionAboveMedia = false) 
        => new(fileType, null, null, stream, fileName, hasSpoiler, showCaptionAboveMedia);
}