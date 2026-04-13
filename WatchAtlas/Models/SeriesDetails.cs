namespace WatchAtlas.Models;

public class SeriesDetails
{
    public Guid MediaItemId { get; set; }
    public string? Universe { get; set; }
    public List<Season> Seasons { get; set; } = new();
}
