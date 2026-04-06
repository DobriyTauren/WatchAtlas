namespace WatchAtlas.Models.Library;

public class GlobalStatistics
{
    public int TotalMovies { get; init; }
    public int WatchedMovies { get; init; }
    public int TotalSeries { get; init; }
    public int CompletedSeries { get; init; }
    public int WatchedEpisodes { get; init; }
    public int TotalEpisodes { get; init; }
    public int TotalWatchTimeMinutes { get; init; }
    public double AverageSeriesCompletionPercent { get; init; }
}
