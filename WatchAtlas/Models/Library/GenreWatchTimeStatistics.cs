namespace WatchAtlas.Models.Library;

public class GenreWatchTimeStatistics
{
    public string Genre { get; init; } = string.Empty;
    public int TotalWatchedMinutes { get; init; }
    public int WatchedMovieMinutes { get; init; }
    public int WatchedEpisodeMinutes { get; init; }
    public int TitleCount { get; init; }
}
