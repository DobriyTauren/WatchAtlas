using System.Text.Json;
using Microsoft.JSInterop;
using WatchAtlas.Helpers;
using WatchAtlas.Models;
using WatchAtlas.Models.Library;

namespace WatchAtlas.Repositories;

public class LocalStorageMediaRepository(IJSRuntime jsRuntime) : IMediaRepository
{
    private const string StorageKey = "watchatlas.library";

    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);
    private LibraryStorageModel? _cache;

    public async Task<List<MediaItem>> GetAllAsync()
    {
        var storage = await GetStorageAsync();

        return storage.MediaItems
            .OrderByDescending(item => item.UpdatedAt)
            .Select(LibraryCloneHelper.Clone)
            .ToList();
    }

    public async Task<MediaItem?> GetByIdAsync(Guid id)
    {
        var storage = await GetStorageAsync();
        var item = storage.MediaItems.FirstOrDefault(candidate => candidate.Id == id);

        return item is null ? null : LibraryCloneHelper.Clone(item);
    }

    public async Task AddAsync(MediaItem item)
    {
        var storage = await GetStorageAsync();
        var normalized = NormalizeMediaItem(item);

        if (storage.MediaItems.Any(candidate => candidate.Id == normalized.Id))
        {
            throw new InvalidOperationException($"A media item with id '{normalized.Id}' already exists.");
        }

        storage.MediaItems.Add(normalized);
        await PersistAsync(storage);
    }

    public async Task UpdateAsync(MediaItem item)
    {
        var storage = await GetStorageAsync();
        var normalized = NormalizeMediaItem(item);
        var existingIndex = storage.MediaItems.FindIndex(candidate => candidate.Id == normalized.Id);

        if (existingIndex < 0)
        {
            throw new InvalidOperationException($"A media item with id '{normalized.Id}' does not exist.");
        }

        storage.MediaItems[existingIndex] = normalized;
        await PersistAsync(storage);
    }

    public async Task DeleteAsync(Guid id)
    {
        var storage = await GetStorageAsync();

        storage.MediaItems.RemoveAll(item => item.Id == id);
        storage.Movies.RemoveAll(details => details.MediaItemId == id);
        storage.Series.RemoveAll(details => details.MediaItemId == id);

        await PersistAsync(storage);
    }

    public async Task<MovieDetails?> GetMovieDetailsAsync(Guid mediaItemId)
    {
        var storage = await GetStorageAsync();
        var details = storage.Movies.FirstOrDefault(candidate => candidate.MediaItemId == mediaItemId);

        return LibraryCloneHelper.Clone(details);
    }

    public async Task SaveMovieDetailsAsync(MovieDetails details)
    {
        var storage = await GetStorageAsync();
        EnsureMediaExists(storage, details.MediaItemId);

        var normalized = NormalizeMovieDetails(details);
        var existingIndex = storage.Movies.FindIndex(candidate => candidate.MediaItemId == normalized.MediaItemId);

        if (existingIndex >= 0)
        {
            storage.Movies[existingIndex] = normalized;
        }
        else
        {
            storage.Movies.Add(normalized);
        }

        storage.Series.RemoveAll(candidate => candidate.MediaItemId == normalized.MediaItemId);
        await PersistAsync(storage);
    }

    public async Task<SeriesDetails?> GetSeriesDetailsAsync(Guid mediaItemId)
    {
        var storage = await GetStorageAsync();
        var details = storage.Series.FirstOrDefault(candidate => candidate.MediaItemId == mediaItemId);

        return LibraryCloneHelper.Clone(details);
    }

    public async Task SaveSeriesDetailsAsync(SeriesDetails details)
    {
        var storage = await GetStorageAsync();
        EnsureMediaExists(storage, details.MediaItemId);

        var normalized = NormalizeSeriesDetails(details);
        var existingIndex = storage.Series.FindIndex(candidate => candidate.MediaItemId == normalized.MediaItemId);

        if (existingIndex >= 0)
        {
            storage.Series[existingIndex] = normalized;
        }
        else
        {
            storage.Series.Add(normalized);
        }

        storage.Movies.RemoveAll(candidate => candidate.MediaItemId == normalized.MediaItemId);
        await PersistAsync(storage);
    }

    public async Task ResetAsync()
    {
        _cache = LibraryStorageModel.CreateDefault();
        await jsRuntime.InvokeVoidAsync("watchAtlasStorage.removeItem", StorageKey);
    }

    private async Task<LibraryStorageModel> GetStorageAsync()
    {
        if (_cache is not null)
        {
            return _cache;
        }

        var json = await jsRuntime.InvokeAsync<string?>("watchAtlasStorage.getItem", StorageKey);
        _cache = LibraryStorageMigrationHelper.Deserialize(json, JsonOptions);

        return _cache;
    }

    private async Task PersistAsync(LibraryStorageModel storage)
    {
        storage.SchemaVersion = LibraryStorageModel.CurrentSchemaVersion;
        _cache = LibraryCloneHelper.Clone(storage);

        var json = JsonSerializer.Serialize(_cache, JsonOptions);
        await jsRuntime.InvokeVoidAsync("watchAtlasStorage.setItem", StorageKey, json);
    }

    private static void EnsureMediaExists(LibraryStorageModel storage, Guid mediaItemId)
    {
        if (storage.MediaItems.All(item => item.Id != mediaItemId))
        {
            throw new InvalidOperationException($"A media item with id '{mediaItemId}' does not exist.");
        }
    }

    private static MediaItem NormalizeMediaItem(MediaItem item)
    {
        var normalized = LibraryCloneHelper.Clone(item);

        normalized.Id = normalized.Id == Guid.Empty ? Guid.NewGuid() : normalized.Id;
        normalized.Title = normalized.Title.Trim();
        normalized.CoverImageUrl = NormalizeString(normalized.CoverImageUrl);
        normalized.Description = NormalizeString(normalized.Description);
        normalized.Notes = NormalizeString(normalized.Notes);
        normalized.PersonalRating = normalized.PersonalRating is >= 1 and <= 10 ? normalized.PersonalRating : null;
        normalized.CreatedAt = normalized.CreatedAt == default ? DateTime.UtcNow : normalized.CreatedAt;
        normalized.UpdatedAt = normalized.UpdatedAt == default ? normalized.CreatedAt : normalized.UpdatedAt;
        normalized.Genres = normalized.Genres
            .Select(genre => genre.Trim())
            .Where(genre => !string.IsNullOrWhiteSpace(genre))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();

        return normalized;
    }

    private static MovieDetails NormalizeMovieDetails(MovieDetails details)
    {
        var normalized = LibraryCloneHelper.Clone(details)!;

        if (normalized.DurationMinutes <= 0)
        {
            normalized.DurationMinutes = null;
        }

        if (!normalized.IsWatched)
        {
            normalized.WatchedDate = null;
        }

        return normalized;
    }

    private static SeriesDetails NormalizeSeriesDetails(SeriesDetails details)
    {
        var normalized = LibraryCloneHelper.Clone(details)!;

        normalized.Seasons = normalized.Seasons
            .OrderBy(season => season.SeasonNumber)
            .Select(season =>
            {
                season.Id = season.Id == Guid.Empty ? Guid.NewGuid() : season.Id;
                season.SeriesId = normalized.MediaItemId;
                season.Title = NormalizeString(season.Title);
                season.Episodes = season.Episodes
                    .OrderBy(episode => episode.EpisodeNumber)
                    .Select(episode =>
                    {
                        episode.Id = episode.Id == Guid.Empty ? Guid.NewGuid() : episode.Id;
                        episode.SeasonId = season.Id;
                        episode.Title = NormalizeString(episode.Title);

                        if (episode.DurationMinutes <= 0)
                        {
                            episode.DurationMinutes = null;
                        }

                        if (!episode.IsWatched)
                        {
                            episode.WatchedDate = null;
                        }

                        return episode;
                    })
                    .ToList();

                return season;
            })
            .ToList();

        return normalized;
    }

    private static string? NormalizeString(string? value)
        => string.IsNullOrWhiteSpace(value) ? null : value.Trim();
}
