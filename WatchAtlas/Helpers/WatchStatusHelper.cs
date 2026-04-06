using WatchAtlas.Models;
using WatchAtlas.Models.Enums;
using WatchAtlas.Models.Library;

namespace WatchAtlas.Helpers;

public static class WatchStatusHelper
{
    public static WatchStatus GetStatus(LibraryEntry entry)
    {
        if (entry.Media.Type == MediaType.Movie)
        {
            return entry.Movie?.IsWatched == true
                ? WatchStatus.Completed
                : WatchStatus.NotStarted;
        }

        var episodes = entry.Series?.Seasons.SelectMany(season => season.Episodes).ToList() ?? new List<Episode>();
        if (episodes.Count == 0)
        {
            return WatchStatus.NotStarted;
        }

        var watchedCount = episodes.Count(episode => episode.IsWatched);
        return watchedCount switch
        {
            0 => WatchStatus.NotStarted,
            var count when count == episodes.Count => WatchStatus.Completed,
            _ => WatchStatus.InProgress
        };
    }
}
