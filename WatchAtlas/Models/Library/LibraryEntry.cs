namespace WatchAtlas.Models.Library;

public class LibraryEntry
{
    public required MediaItem Media { get; init; }
    public MovieDetails? Movie { get; init; }
    public SeriesDetails? Series { get; init; }
}
