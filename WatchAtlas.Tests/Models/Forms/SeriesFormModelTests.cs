using WatchAtlas.Models.Forms;
using Xunit;

namespace WatchAtlas.Tests.Models.Forms;

public class SeriesFormModelTests
{
    [Fact]
    public void MarkSeriesWatched_MarksEveryEpisodeAndPreservesExistingDates()
    {
        var preservedDate = new DateTime(2024, 12, 15);
        var form = new SeriesFormModel
        {
            Seasons =
            [
                new SeasonFormModel
                {
                    SeasonNumber = 1,
                    Episodes =
                    [
                        new EpisodeFormModel { EpisodeNumber = 1 },
                        new EpisodeFormModel { EpisodeNumber = 2, IsWatched = true, WatchedDate = preservedDate }
                    ]
                },
                new SeasonFormModel
                {
                    SeasonNumber = 2,
                    Episodes =
                    [
                        new EpisodeFormModel { EpisodeNumber = 1 }
                    ]
                }
            ]
        };

        form.MarkSeriesWatched();

        Assert.All(
            form.Seasons.SelectMany(season => season.Episodes),
            episode => Assert.True(episode.IsWatched));

        Assert.All(
            form.Seasons.SelectMany(season => season.Episodes),
            episode => Assert.NotNull(episode.WatchedDate));

        Assert.Equal(preservedDate, form.Seasons[0].Episodes[1].WatchedDate);
    }
}
