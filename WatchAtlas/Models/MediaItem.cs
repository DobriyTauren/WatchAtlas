using WatchAtlas.Models.Enums;

namespace WatchAtlas.Models;

public class MediaItem
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Title { get; set; } = string.Empty;
    public MediaType Type { get; set; }
    public string? CoverImageUrl { get; set; }
    public string? Description { get; set; }
    public List<string> Genres { get; set; } = new();
    public int? PersonalRating { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
