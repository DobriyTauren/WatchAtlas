using WatchAtlas.Models.Enums;
using WatchAtlas.State;
using WatchAtlas.Tests.Support;
using Xunit;

namespace WatchAtlas.Tests.State;

public class FilterStateTests
{
    [Fact]
    public void Apply_CombinesSearchGenreTypeAndStatusFilters()
    {
        var state = new FilterState();
        var entries = new[]
        {
            TestDataFactory.CreateSeriesEntry(
                "Dexter",
                genres: ["Crime", "Drama"],
                description: "Forensic analyst and vigilante",
                seasons:
                [
                    TestDataFactory.CreateSeason(
                        1,
                        TestDataFactory.CreateEpisode(1, watched: true, durationMinutes: 55),
                        TestDataFactory.CreateEpisode(2, watched: false, durationMinutes: 52))
                ]),
            TestDataFactory.CreateSeriesEntry(
                "The Boys",
                genres: ["Action"],
                description: "Corrupt superheroes",
                seasons:
                [
                    TestDataFactory.CreateSeason(1, TestDataFactory.CreateEpisode(1, watched: false, durationMinutes: 55))
                ]),
            TestDataFactory.CreateMovieEntry(
                "Prisoners",
                watched: true,
                genres: ["Crime"],
                description: "Missing children thriller")
        };

        state.SetSearchTerm("forensic");
        state.SetGenre("crime");
        state.SetMediaType(MediaType.Series);
        state.SetStatus(WatchStatus.InProgress);

        var result = state.Apply(entries);

        Assert.Equal(3, result.TotalCount);
        Assert.Equal(1, result.FilteredCount);
        Assert.Single(result.Items);
        Assert.Equal("Dexter", result.Items[0].Media.Title);
    }

    [Fact]
    public void Apply_SortsByProgressDescending_ThenByUpdatedDate()
    {
        var state = new FilterState();
        var older = DateTime.UtcNow.AddDays(-3);
        var newer = DateTime.UtcNow.AddDays(-1);

        var entries = new[]
        {
            TestDataFactory.CreateSeriesEntry(
                "Lower Progress",
                updatedAt: newer,
                seasons:
                [
                    TestDataFactory.CreateSeason(1,
                        TestDataFactory.CreateEpisode(1, watched: true, durationMinutes: 55),
                        TestDataFactory.CreateEpisode(2, watched: false, durationMinutes: 55))
                ]),
            TestDataFactory.CreateSeriesEntry(
                "Same Progress Older",
                updatedAt: older,
                seasons:
                [
                    TestDataFactory.CreateSeason(1,
                        TestDataFactory.CreateEpisode(1, watched: true, durationMinutes: 55),
                        TestDataFactory.CreateEpisode(2, watched: true, durationMinutes: 55),
                        TestDataFactory.CreateEpisode(3, watched: false, durationMinutes: 55))
                ]),
            TestDataFactory.CreateSeriesEntry(
                "Same Progress Newer",
                updatedAt: newer,
                seasons:
                [
                    TestDataFactory.CreateSeason(1,
                        TestDataFactory.CreateEpisode(1, watched: true, durationMinutes: 55),
                        TestDataFactory.CreateEpisode(2, watched: true, durationMinutes: 55),
                        TestDataFactory.CreateEpisode(3, watched: false, durationMinutes: 55))
                ])
        };

        state.SetSortBy(LibrarySortBy.Progress);
        state.SetDescending(true);

        var result = state.Apply(entries);

        Assert.Equal(["Same Progress Newer", "Same Progress Older", "Lower Progress"], result.Items.Select(item => item.Media.Title));
    }

    [Fact]
    public void Apply_SortsByRatingAscending_WhenDescendingIsDisabled()
    {
        var state = new FilterState();
        var entries = new[]
        {
            TestDataFactory.CreateMovieEntry("High", rating: 9),
            TestDataFactory.CreateMovieEntry("Missing", rating: null),
            TestDataFactory.CreateMovieEntry("Mid", rating: 5)
        };

        state.SetSortBy(LibrarySortBy.Rating);
        state.SetDescending(false);

        var result = state.Apply(entries);

        Assert.Equal(["Missing", "Mid", "High"], result.Items.Select(item => item.Media.Title));
    }
}
