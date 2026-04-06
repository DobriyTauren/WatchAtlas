using WatchAtlas.Models;

namespace WatchAtlas.Models.Library;

public class LibraryStorageModel
{
    public const int CurrentSchemaVersion = 1;

    public int SchemaVersion { get; set; } = CurrentSchemaVersion;
    public List<MediaItem> MediaItems { get; set; } = new();
    public List<MovieDetails> Movies { get; set; } = new();
    public List<SeriesDetails> Series { get; set; } = new();

    public static LibraryStorageModel CreateDefault() => new()
    {
        SchemaVersion = CurrentSchemaVersion
    };
}
