using WatchAtlas.Models;
using WatchAtlas.Models.Enums;
using WatchAtlas.Models.Library;
using WatchAtlas.Repositories;
using WatchAtlas.Tests.Support;
using Xunit;

namespace WatchAtlas.Tests.Repositories;

public class LocalStorageMediaRepositoryTests
{
    [Fact]
    public async Task AddAndSaveMovieDetails_RoundTripsAndNormalizesStoredValues()
    {
        var jsRuntime = new FakeJsRuntime();
        var repository = new LocalStorageMediaRepository(jsRuntime);
        var mediaId = Guid.NewGuid();

        await repository.AddAsync(new MediaItem
        {
            Id = mediaId,
            Title = "  The Bear  ",
            Type = MediaType.Movie,
            Genres = ["Drama", " drama ", " ", "Comedy"],
            PersonalRating = 14
        });

        await repository.SaveMovieDetailsAsync(new MovieDetails
        {
            MediaItemId = mediaId,
            DurationMinutes = -10,
            IsWatched = false,
            WatchedDate = DateTime.UtcNow
        });

        var storedMedia = await repository.GetByIdAsync(mediaId);
        var storedDetails = await repository.GetMovieDetailsAsync(mediaId);
        var rawJson = jsRuntime.GetStoredValue("watchatlas.library");

        Assert.NotNull(storedMedia);
        Assert.Equal("The Bear", storedMedia.Title);
        Assert.Null(storedMedia.PersonalRating);
        Assert.Equal(["Drama", "Comedy"], storedMedia.Genres);

        Assert.NotNull(storedDetails);
        Assert.Null(storedDetails.DurationMinutes);
        Assert.Null(storedDetails.WatchedDate);
        Assert.Contains("\"mediaItems\"", rawJson);
        Assert.Contains("\"movies\"", rawJson);
    }

    [Fact]
    public async Task DeleteAsync_RemovesMediaAndRelatedDetails()
    {
        var jsRuntime = new FakeJsRuntime();
        var repository = new LocalStorageMediaRepository(jsRuntime);
        var mediaId = Guid.NewGuid();

        await repository.AddAsync(new MediaItem
        {
            Id = mediaId,
            Title = "Delete Me",
            Type = MediaType.Movie
        });

        await repository.SaveMovieDetailsAsync(new MovieDetails
        {
            MediaItemId = mediaId,
            DurationMinutes = 100,
            IsWatched = true
        });

        await repository.DeleteAsync(mediaId);

        Assert.Null(await repository.GetByIdAsync(mediaId));
        Assert.Null(await repository.GetMovieDetailsAsync(mediaId));
        Assert.Empty(await repository.GetAllAsync());
    }

    [Fact]
    public async Task GetAllAsync_WithCorruptedStorage_ReturnsDefaultEmptyLibrary()
    {
        var jsRuntime = new FakeJsRuntime();
        jsRuntime.Seed("watchatlas.library", "{ definitely-not-json");
        var repository = new LocalStorageMediaRepository(jsRuntime);

        var items = await repository.GetAllAsync();
        var storage = await repository.ExportAsync();

        Assert.Empty(items);
        Assert.Equal(LibraryStorageModel.CurrentSchemaVersion, storage.SchemaVersion);
        Assert.Empty(storage.MediaItems);
        Assert.Empty(storage.Movies);
        Assert.Empty(storage.Series);
    }

    [Fact]
    public async Task ReplaceAllAsync_NormalizesSeriesIdsAndClearsUnwatchedDates()
    {
        var jsRuntime = new FakeJsRuntime();
        var repository = new LocalStorageMediaRepository(jsRuntime);
        var mediaId = Guid.NewGuid();

        await repository.ReplaceAllAsync(new LibraryStorageModel
        {
            MediaItems =
            [
                new MediaItem
                {
                    Id = mediaId,
                    Title = "Dexter",
                    Type = MediaType.Series
                }
            ],
            Series =
            [
                new SeriesDetails
                {
                    MediaItemId = mediaId,
                    Seasons =
                    [
                        new Season
                        {
                            Id = Guid.Empty,
                            SeasonNumber = 1,
                            Episodes =
                            [
                                new Episode
                                {
                                    Id = Guid.Empty,
                                    EpisodeNumber = 1,
                                    DurationMinutes = 0,
                                    IsWatched = false,
                                    WatchedDate = DateTime.UtcNow
                                }
                            ]
                        }
                    ]
                }
            ]
        });

        var details = await repository.GetSeriesDetailsAsync(mediaId);

        Assert.NotNull(details);
        var season = Assert.Single(details.Seasons);
        var episode = Assert.Single(season.Episodes);
        Assert.NotEqual(Guid.Empty, season.Id);
        Assert.Equal(mediaId, season.SeriesId);
        Assert.NotEqual(Guid.Empty, episode.Id);
        Assert.Equal(season.Id, episode.SeasonId);
        Assert.Null(episode.DurationMinutes);
        Assert.Null(episode.WatchedDate);
    }
}
