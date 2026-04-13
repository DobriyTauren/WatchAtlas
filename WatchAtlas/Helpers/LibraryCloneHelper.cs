using WatchAtlas.Models;
using WatchAtlas.Models.Library;

namespace WatchAtlas.Helpers;

public static class LibraryCloneHelper
{
    public static AppSettings Clone(AppSettings source) => new()
    {
        ThemeMode = source.ThemeMode,
        Language = source.Language,
        LibraryViewMode = source.LibraryViewMode,
        DefaultLibrarySortBy = source.DefaultLibrarySortBy,
        DefaultLibrarySortDescending = source.DefaultLibrarySortDescending
    };

    public static MediaItem Clone(MediaItem source) => new()
    {
        Id = source.Id,
        Title = source.Title,
        Type = source.Type,
        CoverImageUrl = source.CoverImageUrl,
        Description = source.Description,
        Genres = source.Genres.ToList(),
        PersonalRating = source.PersonalRating,
        CreatedAt = source.CreatedAt,
        UpdatedAt = source.UpdatedAt
    };

    public static MovieDetails? Clone(MovieDetails? source)
        => source is null
            ? null
            : new MovieDetails
            {
                MediaItemId = source.MediaItemId,
                Universe = source.Universe,
                DurationMinutes = source.DurationMinutes,
                IsWatched = source.IsWatched,
                WatchedDate = source.WatchedDate
            };

    public static SeriesDetails? Clone(SeriesDetails? source)
        => source is null
            ? null
            : new SeriesDetails
            {
                MediaItemId = source.MediaItemId,
                Universe = source.Universe,
                Seasons = source.Seasons.Select(Clone).ToList()
            };

    public static Season Clone(Season source) => new()
    {
        Id = source.Id,
        SeriesId = source.SeriesId,
        SeasonNumber = source.SeasonNumber,
        Title = source.Title,
        Episodes = source.Episodes.Select(Clone).ToList()
    };

    public static Episode Clone(Episode source) => new()
    {
        Id = source.Id,
        SeasonId = source.SeasonId,
        EpisodeNumber = source.EpisodeNumber,
        Title = source.Title,
        DurationMinutes = source.DurationMinutes,
        IsWatched = source.IsWatched,
        WatchedDate = source.WatchedDate
    };

    public static LibraryEntry Clone(LibraryEntry source) => new()
    {
        Media = Clone(source.Media),
        Movie = Clone(source.Movie),
        Series = Clone(source.Series)
    };

    public static LibraryStorageModel Clone(LibraryStorageModel source) => new()
    {
        SchemaVersion = source.SchemaVersion,
        MediaItems = source.MediaItems.Select(Clone).ToList(),
        Movies = source.Movies.Select(details => Clone(details)!).ToList(),
        Series = source.Series.Select(details => Clone(details)!).ToList()
    };

    public static LibraryBackupModel Clone(LibraryBackupModel source) => new()
    {
        BackupVersion = source.BackupVersion,
        ExportedAtUtc = source.ExportedAtUtc,
        Library = Clone(source.Library),
        Settings = Clone(source.Settings)
    };
}
