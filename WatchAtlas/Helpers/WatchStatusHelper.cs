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

    public static double GetCompletionPercent(LibraryEntry entry)
    {
        if (entry.Media.Type == MediaType.Movie)
        {
            return entry.Movie?.IsWatched == true ? 100 : 0;
        }

        var episodes = entry.Series?.Seasons.SelectMany(season => season.Episodes).ToList() ?? new List<Episode>();
        if (episodes.Count == 0)
        {
            return 0;
        }

        var watchedCount = episodes.Count(episode => episode.IsWatched);
        return watchedCount * 100d / episodes.Count;
    }

    public static int GetTrackedMinutes(LibraryEntry entry)
    {
        if (entry.Media.Type == MediaType.Movie)
        {
            return entry.Movie?.DurationMinutes ?? 0;
        }

        return (entry.Series?.Seasons ?? Enumerable.Empty<Season>())
            .SelectMany(season => season.Episodes)
            .Where(episode => episode.IsWatched)
            .Sum(episode => episode.DurationMinutes ?? 0);
    }
}
