using WatchAtlas.Helpers;

namespace WatchAtlas.Extensions;

public static class StatisticFormattingExtensions
{
    public static string ToPercentageLabel(this double value, int decimals = 0)
        => $"{Math.Round(value, decimals).ToString($"F{decimals}", System.Globalization.CultureInfo.CurrentCulture)}%";

    public static string ToCountLabel(this int count, string singular, string? plural = null)
        => LocalizedText.FormatCount(count, singular, plural);
}
