using WatchAtlas.Helpers;
using WatchAtlas.Models.Enums;
using WatchAtlas.Tests.Support;
using Xunit;

namespace WatchAtlas.Tests.Helpers;

public class WatchStatusHelperTests
{
    [Fact]
    public void GetStatus_ForWatchedMovie_ReturnsCompleted()
    {
        var entry = TestDataFactory.CreateMovieEntry("Arrival", watched: true, durationMinutes: 116);

        var status = WatchStatusHelper.GetStatus(entry);
        var completion = WatchStatusHelper.GetCompletionPercent(entry);

        Assert.Equal(WatchStatus.Completed, status);
        Assert.Equal(100d, completion);
    }

    [Fact]
    public void GetStatus_ForSeriesWithoutEpisodes_ReturnsNotStarted()
    {
        var entry = TestDataFactory.CreateSeriesEntry("Empty Series");

        Assert.Equal(WatchStatus.NotStarted, WatchStatusHelper.GetStatus(entry));
        Assert.Equal(0d, WatchStatusHelper.GetCompletionPercent(entry));
    }

    [Fact]
    public void GetStatus_ForPartiallyWatchedSeries_ReturnsInProgress()
    {
        var entry = TestDataFactory.CreateSeriesEntry(
            "Dexter",
            seasons:
            [
                TestDataFactory.CreateSeason(
                    1,
                    TestDataFactory.CreateEpisode(1, watched: true, durationMinutes: 50),
                    TestDataFactory.CreateEpisode(2, watched: false, durationMinutes: 47),
                    TestDataFactory.CreateEpisode(3, watched: true, durationMinutes: 52))
            ]);

        Assert.Equal(WatchStatus.InProgress, WatchStatusHelper.GetStatus(entry));
        Assert.Equal(66.66666666666667d, WatchStatusHelper.GetCompletionPercent(entry), precision: 10);
    }

    [Fact]
    public void GetTrackedMinutes_ForSeries_CountsOnlyWatchedEpisodes()
    {
        var entry = TestDataFactory.CreateSeriesEntry(
            "The Boys",
            seasons:
            [
                TestDataFactory.CreateSeason(
                    1,
                    TestDataFactory.CreateEpisode(1, watched: true, durationMinutes: 60),
                    TestDataFactory.CreateEpisode(2, watched: false, durationMinutes: 60),
                    TestDataFactory.CreateEpisode(3, watched: true, durationMinutes: null))
            ]);

        var trackedMinutes = WatchStatusHelper.GetTrackedMinutes(entry);

        Assert.Equal(60, trackedMinutes);
    }
}
