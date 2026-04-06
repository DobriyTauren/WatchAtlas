using WatchAtlas.Models.Enums;

namespace WatchAtlas.Models.Library;

public class SeasonStatistics
{
    public Guid SeasonId { get; init; }
    public Guid SeriesId { get; init; }
    public string SeriesTitle { get; init; } = string.Empty;
    public int SeasonNumber { get; init; }
    public string? Title { get; init; }
    public int TotalEpisodes { get; init; }
    public int WatchedEpisodes { get; init; }
    public int UnwatchedEpisodes { get; init; }
    public int TotalMinutes { get; init; }
    public int WatchedMinutes { get; init; }
    public int RemainingMinutes { get; init; }
    public double CompletionPercent { get; init; }
    public WatchStatus Status { get; init; }
}
