using WatchAtlas.Models;
using WatchAtlas.Models.Enums;
using WatchAtlas.Models.Library;

namespace WatchAtlas.Helpers;

public static class LibraryEntryFactory
{
    public static LibraryEntry Create(MediaItem media, MovieDetails? movie, SeriesDetails? series) => new()
    {
        Media = media,
        Movie = media.Type == MediaType.Movie ? movie : null,
        Series = media.Type == MediaType.Series ? series : null
    };

    public static IReadOnlyList<LibraryEntry> CreateRange(
        IEnumerable<MediaItem> mediaItems,
        IReadOnlyDictionary<Guid, MovieDetails> movieDetails,
        IReadOnlyDictionary<Guid, SeriesDetails> seriesDetails)
        => mediaItems
            .Select(media => Create(
                media,
                TryGet(movieDetails, media.Id),
                TryGet(seriesDetails, media.Id)))
            .ToList();

    private static TValue? TryGet<TValue>(IReadOnlyDictionary<Guid, TValue> source, Guid id)
        where TValue : class
        => source.TryGetValue(id, out var value) ? value : null;
}
