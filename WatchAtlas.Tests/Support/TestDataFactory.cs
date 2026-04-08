using WatchAtlas.Models;
using WatchAtlas.Models.Enums;
using WatchAtlas.Models.Library;

namespace WatchAtlas.Tests.Support;

internal static class TestDataFactory
{
    public static LibraryEntry CreateMovieEntry(
        string title,
        bool watched = false,
        int? durationMinutes = null,
        int? rating = null,
        IEnumerable<string>? genres = null,
        string? description = null,
        DateTime? createdAt = null,
        DateTime? updatedAt = null,
        Guid? id = null)
    {
        var mediaId = id ?? Guid.NewGuid();

        return new LibraryEntry
        {
            Media = new MediaItem
            {
                Id = mediaId,
                Title = title,
                Type = MediaType.Movie,
                Description = description,
                Genres = genres?.ToList() ?? new(),
                PersonalRating = rating,
                CreatedAt = createdAt ?? DateTime.UtcNow,
                UpdatedAt = updatedAt ?? createdAt ?? DateTime.UtcNow
            },
            Movie = new MovieDetails
            {
                MediaItemId = mediaId,
                DurationMinutes = durationMinutes,
                IsWatched = watched,
                WatchedDate = watched ? DateTime.UtcNow.Date : null
            }
        };
    }

    public static LibraryEntry CreateSeriesEntry(
        string title,
        IEnumerable<Season>? seasons = null,
        int? rating = null,
        IEnumerable<string>? genres = null,
        string? description = null,
        DateTime? createdAt = null,
        DateTime? updatedAt = null,
        Guid? id = null)
    {
        var mediaId = id ?? Guid.NewGuid();
        var seasonList = (seasons ?? Enumerable.Empty<Season>())
            .Select(CloneSeasonForSeries)
            .ToList();

        foreach (var season in seasonList)
        {
            season.SeriesId = mediaId;
        }

        return new LibraryEntry
        {
            Media = new MediaItem
            {
                Id = mediaId,
                Title = title,
                Type = MediaType.Series,
                Description = description,
                Genres = genres?.ToList() ?? new(),
                PersonalRating = rating,
                CreatedAt = createdAt ?? DateTime.UtcNow,
                UpdatedAt = updatedAt ?? createdAt ?? DateTime.UtcNow
            },
            Series = new SeriesDetails
            {
                MediaItemId = mediaId,
                Seasons = seasonList
            }
        };
    }

    public static Season CreateSeason(int seasonNumber, params Episode[] episodes)
    {
        var seasonId = Guid.NewGuid();

        return new Season
        {
            Id = seasonId,
            SeasonNumber = seasonNumber,
            Episodes = episodes.Select(episode => CloneEpisodeForSeason(episode, seasonId)).ToList()
        };
    }

    public static Episode CreateEpisode(int episodeNumber, bool watched = false, int? durationMinutes = null, string? title = null)
    {
        return new Episode
        {
            Id = Guid.NewGuid(),
            EpisodeNumber = episodeNumber,
            Title = title ?? $"Episode {episodeNumber}",
            DurationMinutes = durationMinutes,
            IsWatched = watched,
            WatchedDate = watched ? DateTime.UtcNow.Date : null
        };
    }

    private static Season CloneSeasonForSeries(Season source)
    {
        var seasonId = source.Id == Guid.Empty ? Guid.NewGuid() : source.Id;

        return new Season
        {
            Id = seasonId,
            SeriesId = source.SeriesId,
            SeasonNumber = source.SeasonNumber,
            Title = source.Title,
            Episodes = source.Episodes.Select(episode => CloneEpisodeForSeason(episode, seasonId)).ToList()
        };
    }

    private static Episode CloneEpisodeForSeason(Episode source, Guid seasonId)
    {
        return new Episode
        {
            Id = source.Id == Guid.Empty ? Guid.NewGuid() : source.Id,
            SeasonId = seasonId,
            EpisodeNumber = source.EpisodeNumber,
            Title = source.Title,
            DurationMinutes = source.DurationMinutes,
            IsWatched = source.IsWatched,
            WatchedDate = source.WatchedDate
        };
    }
}
