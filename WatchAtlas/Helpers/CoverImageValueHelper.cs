namespace WatchAtlas.Helpers;

public static class CoverImageValueHelper
{
    public static string? Normalize(string? rawValue)
    {
        var normalized = rawValue?.Trim();
        return string.IsNullOrWhiteSpace(normalized) ? null : normalized;
    }

    public static bool IsExternalUrl(string? rawValue)
    {
        var normalized = Normalize(rawValue);
        return normalized is not null
            && Uri.TryCreate(normalized, UriKind.Absolute, out var uri)
            && (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps);
    }

    public static bool IsEmbeddedImage(string? rawValue)
    {
        var normalized = Normalize(rawValue);
        return normalized?.StartsWith("data:image/", StringComparison.OrdinalIgnoreCase) == true;
    }

    public static bool IsValidStoredValue(string? rawValue)
    {
        var normalized = Normalize(rawValue);
        return normalized is null || IsExternalUrl(normalized) || IsEmbeddedImage(normalized);
    }
}
