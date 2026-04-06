namespace WatchAtlas.Models.Library;

public class WatchTimeSummary
{
    public int WatchedMovieMinutes { get; init; }
    public int WatchedEpisodeMinutes { get; init; }
    public int TotalWatchedMinutes { get; init; }
    public int TotalTrackedMinutes { get; init; }
    public int RemainingMinutes { get; init; }
}
