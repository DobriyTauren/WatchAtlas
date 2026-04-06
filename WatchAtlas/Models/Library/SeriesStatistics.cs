using WatchAtlas.Models.Enums;

namespace WatchAtlas.Models.Library;

public class SeriesStatistics
{
    public Guid SeriesId { get; init; }
    public string Title { get; init; } = string.Empty;
    public string? CoverImageUrl { get; init; }
    public int SeasonCount { get; init; }
    public int TotalEpisodes { get; init; }
    public int WatchedEpisodes { get; init; }
    public int UnwatchedEpisodes { get; init; }
    public int TotalMinutes { get; init; }
    public int WatchedMinutes { get; init; }
    public int RemainingMinutes { get; init; }
    public double CompletionPercent { get; init; }
    public WatchStatus Status { get; init; }
    public IReadOnlyList<SeasonStatistics> Seasons { get; init; } = Array.Empty<SeasonStatistics>();
}
