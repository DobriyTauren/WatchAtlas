namespace WatchAtlas.Models;

public class MovieDetails
{
    public Guid MediaItemId { get; set; }
    public int? DurationMinutes { get; set; }
    public bool IsWatched { get; set; }
    public DateTime? WatchedDate { get; set; }
}
