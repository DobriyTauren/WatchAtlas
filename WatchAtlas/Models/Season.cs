namespace WatchAtlas.Models;

public class Season
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid SeriesId { get; set; }
    public int SeasonNumber { get; set; }
    public string? Title { get; set; }
    public List<Episode> Episodes { get; set; } = new();
}
