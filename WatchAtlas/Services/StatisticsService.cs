using WatchAtlas.Helpers;
using WatchAtlas.Models;
using WatchAtlas.Models.Enums;
using WatchAtlas.Models.Library;

namespace WatchAtlas.Services;

public class StatisticsService : IStatisticsService
{
    public GlobalStatistics CalculateGlobalStatistics(IEnumerable<LibraryEntry> entries)
    {
        var libraryEntries = entries.ToList();
        var seriesEntries = libraryEntries.Where(entry => entry.Media.Type == MediaType.Series).ToList();
        var watchedMovieMinutes = libraryEntries
            .Where(entry => entry.Media.Type == MediaType.Movie && entry.Movie?.IsWatched == true)
            .Sum(entry => entry.Movie?.DurationMinutes ?? 0);

        var allEpisodes = seriesEntries
            .SelectMany(entry => entry.Series?.Seasons ?? Enumerable.Empty<Season>())
            .SelectMany(season => season.Episodes)
            .ToList();

        var watchedEpisodes = allEpisodes.Where(episode => episode.IsWatched).ToList();
        var completedSeries = seriesEntries.Count(entry => WatchStatusHelper.GetStatus(entry) == WatchStatus.Completed);
        var averageCompletion = seriesEntries.Count == 0
            ? 0
            : seriesEntries.Average(entry => CalculateSeriesProgress(entry).CompletionPercent);

        return new GlobalStatistics
        {
            TotalMovies = libraryEntries.Count(entry => entry.Media.Type == MediaType.Movie),
            WatchedMovies = libraryEntries.Count(entry => entry.Media.Type == MediaType.Movie && entry.Movie?.IsWatched == true),
            TotalSeries = seriesEntries.Count,
            CompletedSeries = completedSeries,
            WatchedEpisodes = watchedEpisodes.Count,
            TotalEpisodes = allEpisodes.Count,
            TotalWatchTimeMinutes = watchedMovieMinutes + watchedEpisodes.Sum(episode => episode.DurationMinutes ?? 0),
            AverageSeriesCompletionPercent = averageCompletion
        };
    }

    public SeriesProgressSummary CalculateSeriesProgress(LibraryEntry entry)
    {
        var episodes = entry.Series?.Seasons.SelectMany(season => season.Episodes).ToList() ?? new List<Episode>();
        var watchedEpisodes = episodes.Where(episode => episode.IsWatched).ToList();
        var totalEpisodes = episodes.Count;

        return new SeriesProgressSummary
        {
            WatchedEpisodes = watchedEpisodes.Count,
            TotalEpisodes = totalEpisodes,
            WatchedMinutes = watchedEpisodes.Sum(episode => episode.DurationMinutes ?? 0),
            CompletionPercent = totalEpisodes == 0 ? 0 : watchedEpisodes.Count * 100d / totalEpisodes
        };
    }
}
