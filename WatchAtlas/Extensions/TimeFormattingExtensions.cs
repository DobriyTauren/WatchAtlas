using WatchAtlas.Helpers;

namespace WatchAtlas.Extensions;

public static class TimeFormattingExtensions
{
    public static string ToReadableDuration(this int minutes)
        => LocalizedText.FormatDuration(minutes);
}
