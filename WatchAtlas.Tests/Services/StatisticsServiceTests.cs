using WatchAtlas.Models.Enums;
using WatchAtlas.Services;
using WatchAtlas.Tests.Support;
using Xunit;

namespace WatchAtlas.Tests.Services;

public class StatisticsServiceTests
{
    private readonly StatisticsService _service = new();

    [Fact]
    public void CalculateGlobalStatistics_ForMixedLibrary_ReturnsExpectedTotals()
    {
        var entries = new[]
        {
            TestDataFactory.CreateMovieEntry("Watched Movie", watched: true, durationMinutes: 120),
            TestDataFactory.CreateMovieEntry("Queued Movie", watched: false, durationMinutes: 95),
            TestDataFactory.CreateSeriesEntry(
                "Dexter",
                seasons:
                [
                    TestDataFactory.CreateSeason(
                        1,
                        TestDataFactory.CreateEpisode(1, watched: true, durationMinutes: 50),
                        TestDataFactory.CreateEpisode(2, watched: false, durationMinutes: 55)),
                    TestDataFactory.CreateSeason(
                        2,
                        TestDataFactory.CreateEpisode(1, watched: true, durationMinutes: 52),
                        TestDataFactory.CreateEpisode(2, watched: false, durationMinutes: 53))
                ])
        };

        var stats = _service.CalculateGlobalStatistics(entries);

        Assert.Equal(2, stats.TotalMovies);
        Assert.Equal(1, stats.WatchedMovies);
        Assert.Equal(1, stats.TotalSeries);
        Assert.Equal(2, stats.WatchedEpisodes);
        Assert.Equal(4, stats.TotalEpisodes);
        Assert.Equal(222, stats.TotalWatchTimeMinutes);
        Assert.Equal(50d, stats.AverageSeriesCompletionPercent);
        Assert.Equal(50d, stats.MovieWatchRatePercent);
        Assert.Equal(50d, stats.EpisodeWatchRatePercent);
        Assert.Equal(120, stats.WatchTime.WatchedMovieMinutes);
        Assert.Equal(102, stats.WatchTime.WatchedEpisodeMinutes);
        Assert.Equal(425, stats.WatchTime.TotalTrackedMinutes);
        Assert.Equal(203, stats.WatchTime.RemainingMinutes);
    }

    [Fact]
    public void CalculateSeriesStatistics_ForSeriesWithoutSeasons_ReturnsNotStartedZeroes()
    {
        var entry = TestDataFactory.CreateSeriesEntry("Skeleton Series");

        var stats = _service.CalculateSeriesStatistics(entry);

        Assert.Equal(0, stats.SeasonCount);
        Assert.Equal(0, stats.TotalEpisodes);
        Assert.Equal(0, stats.WatchedEpisodes);
        Assert.Equal(0d, stats.CompletionPercent);
        Assert.Equal(WatchStatus.NotStarted, stats.Status);
    }

    [Fact]
    public void GetMostEpisodesWatchedSeries_OrdersByWatchedEpisodesThenWatchTime()
    {
        var entries = new[]
        {
            TestDataFactory.CreateSeriesEntry(
                "Gamma",
                seasons:
                [
                    TestDataFactory.CreateSeason(1,
                        TestDataFactory.CreateEpisode(1, watched: true, durationMinutes: 50),
                        TestDataFactory.CreateEpisode(2, watched: true, durationMinutes: 50))
                ]),
            TestDataFactory.CreateSeriesEntry(
                "Beta",
                seasons:
                [
                    TestDataFactory.CreateSeason(1,
                        TestDataFactory.CreateEpisode(1, watched: true, durationMinutes: 50),
                        TestDataFactory.CreateEpisode(2, watched: true, durationMinutes: 50),
                        TestDataFactory.CreateEpisode(3, watched: false, durationMinutes: 50))
                ]),
            TestDataFactory.CreateSeriesEntry(
                "Alpha",
                seasons:
                [
                    TestDataFactory.CreateSeason(1,
                        TestDataFactory.CreateEpisode(1, watched: true, durationMinutes: 50),
                        TestDataFactory.CreateEpisode(2, watched: false, durationMinutes: 50))
                ])
        };

        var ordered = _service.GetMostEpisodesWatchedSeries(entries, 3).Select(item => item.Title).ToList();

        Assert.Equal(["Beta", "Gamma", "Alpha"], ordered);
    }

    [Fact]
    public void GetCurrentlyWatchingSeries_ReturnsOnlyInProgressSeries()
    {
        var entries = new[]
        {
            TestDataFactory.CreateSeriesEntry(
                "Completed",
                seasons:
                [
                    TestDataFactory.CreateSeason(1,
                        TestDataFactory.CreateEpisode(1, watched: true, durationMinutes: 42))
                ]),
            TestDataFactory.CreateSeriesEntry(
                "Currently Watching",
                seasons:
                [
                    TestDataFactory.CreateSeason(1,
                        TestDataFactory.CreateEpisode(1, watched: true, durationMinutes: 44),
                        TestDataFactory.CreateEpisode(2, watched: false, durationMinutes: 44))
                ]),
            TestDataFactory.CreateSeriesEntry("Not Started")
        };

        var ordered = _service.GetCurrentlyWatchingSeries(entries, 3).Select(item => item.Title).ToList();

        Assert.Equal(["Currently Watching"], ordered);
    }

    [Fact]
    public void GetTopGenresByWatchTime_AggregatesMoviesAndEpisodesByGenre()
    {
        var entries = new[]
        {
            TestDataFactory.CreateMovieEntry("Arrival", watched: true, durationMinutes: 116, genres: ["Sci-Fi", "Drama"]),
            TestDataFactory.CreateSeriesEntry(
                "Dark",
                genres: ["Sci-Fi", "Mystery"],
                seasons:
                [
                    TestDataFactory.CreateSeason(1,
                        TestDataFactory.CreateEpisode(1, watched: true, durationMinutes: 50),
                        TestDataFactory.CreateEpisode(2, watched: true, durationMinutes: 55))
                ])
        };

        var genres = _service.GetTopGenresByWatchTime(entries, 3);
        var sciFi = Assert.Single(genres.Where(item => item.Genre == "Sci-Fi"));

        Assert.Equal(221, sciFi.TotalWatchedMinutes);
        Assert.Equal(116, sciFi.WatchedMovieMinutes);
        Assert.Equal(105, sciFi.WatchedEpisodeMinutes);
        Assert.Equal(2, sciFi.TitleCount);
    }
}
