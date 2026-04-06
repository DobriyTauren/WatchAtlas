namespace WatchAtlas.Models.Library;

public class SeriesProgressSummary
{
    public int WatchedEpisodes { get; init; }
    public int TotalEpisodes { get; init; }
    public int WatchedMinutes { get; init; }
    public double CompletionPercent { get; init; }
}
