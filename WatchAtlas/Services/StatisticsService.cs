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
        var seriesStatistics = seriesEntries.Select(CalculateSeriesStatistics).ToList();
        var watchedMovieMinutes = libraryEntries
            .Where(entry => entry.Media.Type == MediaType.Movie && entry.Movie?.IsWatched == true)
            .Sum(entry => entry.Movie?.DurationMinutes ?? 0);
        var completedSeries = seriesEntries.Count(entry => WatchStatusHelper.GetStatus(entry) == WatchStatus.Completed);
        var averageCompletion = seriesStatistics.Count == 0
            ? 0
            : seriesStatistics.Average(stats => stats.CompletionPercent);

        return new GlobalStatistics
        {
            TotalMovies = libraryEntries.Count(entry => entry.Media.Type == MediaType.Movie),
            WatchedMovies = libraryEntries.Count(entry => entry.Media.Type == MediaType.Movie && entry.Movie?.IsWatched == true),
            TotalSeries = seriesEntries.Count,
            CompletedSeries = completedSeries,
            WatchedEpisodes = seriesStatistics.Sum(stats => stats.WatchedEpisodes),
            TotalEpisodes = seriesStatistics.Sum(stats => stats.TotalEpisodes),
            TotalWatchTimeMinutes = watchedMovieMinutes + seriesStatistics.Sum(stats => stats.WatchedMinutes),
            AverageSeriesCompletionPercent = averageCompletion
        };
    }

    public SeriesProgressSummary CalculateSeriesProgress(LibraryEntry entry)
    {
        var statistics = CalculateSeriesStatistics(entry);

        return new SeriesProgressSummary
        {
            WatchedEpisodes = statistics.WatchedEpisodes,
            TotalEpisodes = statistics.TotalEpisodes,
            WatchedMinutes = statistics.WatchedMinutes,
            CompletionPercent = statistics.CompletionPercent
        };
    }

    public SeriesStatistics CalculateSeriesStatistics(LibraryEntry entry)
    {
        var seasonStatistics = (entry.Series?.Seasons ?? Enumerable.Empty<Season>())
            .OrderBy(season => season.SeasonNumber)
            .Select(CalculateSeasonStatistics)
            .ToList();

        var totalEpisodes = seasonStatistics.Sum(stats => stats.TotalEpisodes);
        var watchedEpisodes = seasonStatistics.Sum(stats => stats.WatchedEpisodes);

        return new SeriesStatistics
        {
            SeriesId = entry.Media.Id,
            Title = entry.Media.Title,
            SeasonCount = seasonStatistics.Count,
            TotalEpisodes = totalEpisodes,
            WatchedEpisodes = watchedEpisodes,
            TotalMinutes = seasonStatistics.Sum(stats => stats.TotalMinutes),
            WatchedMinutes = seasonStatistics.Sum(stats => stats.WatchedMinutes),
            CompletionPercent = totalEpisodes == 0 ? 0 : watchedEpisodes * 100d / totalEpisodes,
            Seasons = seasonStatistics
        };
    }

    public SeasonStatistics CalculateSeasonStatistics(Season season)
    {
        var episodes = season.Episodes
            .OrderBy(episode => episode.EpisodeNumber)
            .ToList();

        var watchedEpisodes = episodes.Where(episode => episode.IsWatched).ToList();

        return new SeasonStatistics
        {
            SeasonId = season.Id,
            SeasonNumber = season.SeasonNumber,
            Title = season.Title,
            TotalEpisodes = episodes.Count,
            WatchedEpisodes = watchedEpisodes.Count,
            TotalMinutes = episodes.Sum(episode => episode.DurationMinutes ?? 0),
            WatchedMinutes = watchedEpisodes.Sum(episode => episode.DurationMinutes ?? 0),
            CompletionPercent = episodes.Count == 0 ? 0 : watchedEpisodes.Count * 100d / episodes.Count
        };
    }
}
