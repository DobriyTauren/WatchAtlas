namespace WatchAtlas.Services;

public class ImageService : IImageService
{
    public string? NormalizeCoverImage(string? rawValue)
    {
        var normalized = rawValue?.Trim();
        return string.IsNullOrWhiteSpace(normalized) ? null : normalized;
    }
}
