namespace WatchAtlas.Models;

public class Episode
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid SeasonId { get; set; }
    public int EpisodeNumber { get; set; }
    public string? Title { get; set; }
    public int? DurationMinutes { get; set; }
    public bool IsWatched { get; set; }
    public DateTime? WatchedDate { get; set; }
}
