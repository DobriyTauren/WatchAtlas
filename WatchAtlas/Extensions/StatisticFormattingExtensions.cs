namespace WatchAtlas.Extensions;

public static class StatisticFormattingExtensions
{
    public static string ToPercentageLabel(this double value, int decimals = 0)
        => $"{Math.Round(value, decimals).ToString($"F{decimals}")}%";

    public static string ToCountLabel(this int count, string singular, string? plural = null)
    {
        var pluralLabel = plural ?? $"{singular}s";
        return $"{count} {(count == 1 ? singular : pluralLabel)}";
    }
}
