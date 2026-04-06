using WatchAtlas.Helpers;
using WatchAtlas.Models;
using WatchAtlas.Models.Enums;
using WatchAtlas.Models.Library;
using WatchAtlas.Repositories;

namespace WatchAtlas.State;

public class LibraryState(IMediaRepository repository) : StateStoreBase
{
    public IReadOnlyList<LibraryEntry> Entries { get; private set; } = Array.Empty<LibraryEntry>();
    public bool IsLoaded { get; private set; }
    public bool IsLoading { get; private set; }

    public async Task EnsureLoadedAsync()
    {
        if (IsLoaded || IsLoading)
        {
            return;
        }

        IsLoading = true;
        NotifyStateChanged();

        try
        {
            Entries = await LoadEntriesAsync();
            IsLoaded = true;
        }
        finally
        {
            IsLoading = false;
            NotifyStateChanged();
        }
    }

    public async Task RefreshAsync()
    {
        IsLoading = true;
        NotifyStateChanged();

        try
        {
            Entries = await LoadEntriesAsync();
            IsLoaded = true;
        }
        finally
        {
            IsLoading = false;
            NotifyStateChanged();
        }
    }

    public async Task AddAsync(MediaItem media, MovieDetails? movieDetails, SeriesDetails? seriesDetails)
    {
        var preparedMedia = PrepareMediaItemForCreate(media);
        ApplyPreparedMedia(media, preparedMedia);
        await repository.AddAsync(preparedMedia);
        await SaveDetailsAsync(preparedMedia, movieDetails, seriesDetails);
        await RefreshAsync();
    }

    public async Task UpdateAsync(MediaItem media, MovieDetails? movieDetails, SeriesDetails? seriesDetails)
    {
        var existing = await repository.GetByIdAsync(media.Id);
        if (existing is null)
        {
            throw new InvalidOperationException($"Unable to update media item '{media.Id}' because it does not exist.");
        }

        var preparedMedia = PrepareMediaItemForUpdate(media, existing.CreatedAt);
        ApplyPreparedMedia(media, preparedMedia);
        await repository.UpdateAsync(preparedMedia);
        await SaveDetailsAsync(preparedMedia, movieDetails, seriesDetails);
        await RefreshAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        await repository.DeleteAsync(id);
        await RefreshAsync();
    }

    public async Task ResetAsync()
    {
        await repository.ResetAsync();
        Entries = Array.Empty<LibraryEntry>();
        IsLoaded = true;
        IsLoading = false;
        NotifyStateChanged();
    }

    public LibraryEntry? GetById(Guid id) => Entries.FirstOrDefault(entry => entry.Media.Id == id);

    private async Task<IReadOnlyList<LibraryEntry>> LoadEntriesAsync()
    {
        var mediaItems = await repository.GetAllAsync();
        var movieDetailTasks = mediaItems
            .Where(item => item.Type == MediaType.Movie)
            .Select(async item => new KeyValuePair<Guid, MovieDetails?>(item.Id, await repository.GetMovieDetailsAsync(item.Id)));

        var seriesDetailTasks = mediaItems
            .Where(item => item.Type == MediaType.Series)
            .Select(async item => new KeyValuePair<Guid, SeriesDetails?>(item.Id, await repository.GetSeriesDetailsAsync(item.Id)));

        var movieDetails = (await Task.WhenAll(movieDetailTasks))
            .Where(pair => pair.Value is not null)
            .ToDictionary(pair => pair.Key, pair => pair.Value!);

        var seriesDetails = (await Task.WhenAll(seriesDetailTasks))
            .Where(pair => pair.Value is not null)
            .ToDictionary(pair => pair.Key, pair => pair.Value!);

        return LibraryEntryFactory.CreateRange(mediaItems, movieDetails, seriesDetails);
    }

    private async Task SaveDetailsAsync(MediaItem media, MovieDetails? movieDetails, SeriesDetails? seriesDetails)
    {
        if (media.Type == MediaType.Movie)
        {
            var preparedMovie = PrepareMovieDetails(media.Id, movieDetails);
            await repository.SaveMovieDetailsAsync(preparedMovie);
            return;
        }

        var preparedSeries = PrepareSeriesDetails(media.Id, seriesDetails);
        await repository.SaveSeriesDetailsAsync(preparedSeries);
    }

    private static MediaItem PrepareMediaItemForCreate(MediaItem media)
    {
        var prepared = LibraryCloneHelper.Clone(media);
        var now = DateTime.UtcNow;

        prepared.Id = prepared.Id == Guid.Empty ? Guid.NewGuid() : prepared.Id;
        prepared.CreatedAt = prepared.CreatedAt == default ? now : prepared.CreatedAt;
        prepared.UpdatedAt = now;

        return prepared;
    }

    private static MediaItem PrepareMediaItemForUpdate(MediaItem media, DateTime createdAt)
    {
        var prepared = LibraryCloneHelper.Clone(media);

        prepared.CreatedAt = createdAt;
        prepared.UpdatedAt = DateTime.UtcNow;

        return prepared;
    }

    private static MovieDetails PrepareMovieDetails(Guid mediaItemId, MovieDetails? movieDetails)
    {
        var prepared = LibraryCloneHelper.Clone(movieDetails) ?? new MovieDetails();

        prepared.MediaItemId = mediaItemId;
        if (!prepared.IsWatched)
        {
            prepared.WatchedDate = null;
        }

        return prepared;
    }

    private static void ApplyPreparedMedia(MediaItem target, MediaItem source)
    {
        target.Id = source.Id;
        target.Title = source.Title;
        target.Type = source.Type;
        target.CoverImageUrl = source.CoverImageUrl;
        target.Description = source.Description;
        target.Genres = source.Genres.ToList();
        target.PersonalRating = source.PersonalRating;
        target.Notes = source.Notes;
        target.CreatedAt = source.CreatedAt;
        target.UpdatedAt = source.UpdatedAt;
    }

    private static SeriesDetails PrepareSeriesDetails(Guid mediaItemId, SeriesDetails? seriesDetails)
    {
        var prepared = LibraryCloneHelper.Clone(seriesDetails) ?? new SeriesDetails();

        prepared.MediaItemId = mediaItemId;
        prepared.Seasons ??= new();

        foreach (var season in prepared.Seasons)
        {
            season.Id = season.Id == Guid.Empty ? Guid.NewGuid() : season.Id;
            season.SeriesId = mediaItemId;
            season.Episodes ??= new();

            foreach (var episode in season.Episodes)
            {
                episode.Id = episode.Id == Guid.Empty ? Guid.NewGuid() : episode.Id;
                episode.SeasonId = season.Id;

                if (!episode.IsWatched)
                {
                    episode.WatchedDate = null;
                }
            }
        }

        return prepared;
    }
}
