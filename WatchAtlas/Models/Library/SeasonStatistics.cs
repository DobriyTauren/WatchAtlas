namespace WatchAtlas.Models.Library;

public class SeasonStatistics
{
    public Guid SeasonId { get; init; }
    public int SeasonNumber { get; init; }
    public string? Title { get; init; }
    public int TotalEpisodes { get; init; }
    public int WatchedEpisodes { get; init; }
    public int TotalMinutes { get; init; }
    public int WatchedMinutes { get; init; }
    public double CompletionPercent { get; init; }
}
