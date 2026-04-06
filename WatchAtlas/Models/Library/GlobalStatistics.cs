namespace WatchAtlas.Models.Library;

public class GlobalStatistics
{
    public int TotalMovies { get; init; }
    public int WatchedMovies { get; init; }
    public int UnwatchedMovies { get; init; }
    public int TotalSeries { get; init; }
    public int CompletedSeries { get; init; }
    public int InProgressSeries { get; init; }
    public int NotStartedSeries { get; init; }
    public int WatchedEpisodes { get; init; }
    public int TotalEpisodes { get; init; }
    public int UnwatchedEpisodes { get; init; }
    public int TotalWatchTimeMinutes { get; init; }
    public double AverageSeriesCompletionPercent { get; init; }
    public double MovieWatchRatePercent { get; init; }
    public double EpisodeWatchRatePercent { get; init; }
    public WatchTimeSummary WatchTime { get; init; } = new();
}
