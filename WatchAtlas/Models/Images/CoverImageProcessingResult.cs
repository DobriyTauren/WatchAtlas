namespace WatchAtlas.Models.Images;

public class CoverImageProcessingResult
{
    public bool IsSuccess { get; init; }
    public string? ImageDataUrl { get; init; }
    public string? ErrorMessage { get; init; }
}
