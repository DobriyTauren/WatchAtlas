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
        var seriesStatistics = CalculateSeriesStatistics(seriesEntries);
        var watchedMovieMinutes = libraryEntries
            .Where(entry => entry.Media.Type == MediaType.Movie && entry.Movie?.IsWatched == true)
            .Sum(entry => entry.Movie?.DurationMinutes ?? 0);
        var watchTimeSummary = CalculateWatchTimeSummary(libraryEntries);
        var completedSeries = seriesStatistics.Count(stats => stats.Status == WatchStatus.Completed);
        var inProgressSeries = seriesStatistics.Count(stats => stats.Status == WatchStatus.InProgress);
        var notStartedSeries = seriesStatistics.Count(stats => stats.Status == WatchStatus.NotStarted);
        var averageCompletion = seriesStatistics.Count == 0
            ? 0
            : seriesStatistics.Average(stats => stats.CompletionPercent);
        var watchedMovies = libraryEntries.Count(entry => entry.Media.Type == MediaType.Movie && entry.Movie?.IsWatched == true);
        var totalMovies = libraryEntries.Count(entry => entry.Media.Type == MediaType.Movie);
        var watchedEpisodes = seriesStatistics.Sum(stats => stats.WatchedEpisodes);
        var totalEpisodes = seriesStatistics.Sum(stats => stats.TotalEpisodes);

        return new GlobalStatistics
        {
            TotalMovies = totalMovies,
            WatchedMovies = watchedMovies,
            UnwatchedMovies = Math.Max(totalMovies - watchedMovies, 0),
            TotalSeries = seriesEntries.Count,
            CompletedSeries = completedSeries,
            InProgressSeries = inProgressSeries,
            NotStartedSeries = notStartedSeries,
            WatchedEpisodes = watchedEpisodes,
            TotalEpisodes = totalEpisodes,
            UnwatchedEpisodes = Math.Max(totalEpisodes - watchedEpisodes, 0),
            TotalWatchTimeMinutes = watchedMovieMinutes + seriesStatistics.Sum(stats => stats.WatchedMinutes),
            AverageSeriesCompletionPercent = averageCompletion,
            MovieWatchRatePercent = totalMovies == 0 ? 0 : watchedMovies * 100d / totalMovies,
            EpisodeWatchRatePercent = totalEpisodes == 0 ? 0 : watchedEpisodes * 100d / totalEpisodes,
            WatchTime = watchTimeSummary
        };
    }

    public IReadOnlyList<SeriesStatistics> CalculateSeriesStatistics(IEnumerable<LibraryEntry> entries)
    {
        return entries
            .Where(entry => entry.Media.Type == MediaType.Series)
            .Select(CalculateSeriesStatistics)
            .OrderByDescending(stats => stats.WatchedMinutes)
            .ThenBy(stats => stats.Title)
            .ToList();
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
            .Select(season => CalculateSeasonStatistics(season, entry.Media.Id, entry.Media.Title))
            .ToList();

        var totalEpisodes = seasonStatistics.Sum(stats => stats.TotalEpisodes);
        var watchedEpisodes = seasonStatistics.Sum(stats => stats.WatchedEpisodes);

        return new SeriesStatistics
        {
            SeriesId = entry.Media.Id,
            Title = entry.Media.Title,
            CoverImageUrl = entry.Media.CoverImageUrl,
            SeasonCount = seasonStatistics.Count,
            TotalEpisodes = totalEpisodes,
            WatchedEpisodes = watchedEpisodes,
            UnwatchedEpisodes = Math.Max(totalEpisodes - watchedEpisodes, 0),
            TotalMinutes = seasonStatistics.Sum(stats => stats.TotalMinutes),
            WatchedMinutes = seasonStatistics.Sum(stats => stats.WatchedMinutes),
            RemainingMinutes = seasonStatistics.Sum(stats => stats.RemainingMinutes),
            CompletionPercent = totalEpisodes == 0 ? 0 : watchedEpisodes * 100d / totalEpisodes,
            Status = WatchStatusHelper.GetStatus(entry),
            Seasons = seasonStatistics
        };
    }

    public SeasonStatistics CalculateSeasonStatistics(Season season)
        => CalculateSeasonStatistics(season, Guid.Empty, string.Empty);

    public WatchTimeSummary CalculateWatchTimeSummary(IEnumerable<LibraryEntry> entries)
    {
        var libraryEntries = entries.ToList();
        var movieMinutes = libraryEntries
            .Where(entry => entry.Media.Type == MediaType.Movie && entry.Movie?.IsWatched == true)
            .Sum(entry => entry.Movie?.DurationMinutes ?? 0);
        var trackedMovieMinutes = libraryEntries
            .Where(entry => entry.Media.Type == MediaType.Movie)
            .Sum(entry => entry.Movie?.DurationMinutes ?? 0);
        var seriesStatistics = CalculateSeriesStatistics(libraryEntries);
        var watchedEpisodeMinutes = seriesStatistics.Sum(stats => stats.WatchedMinutes);
        var trackedEpisodeMinutes = seriesStatistics.Sum(stats => stats.TotalMinutes);

        return new WatchTimeSummary
        {
            WatchedMovieMinutes = movieMinutes,
            WatchedEpisodeMinutes = watchedEpisodeMinutes,
            TotalWatchedMinutes = movieMinutes + watchedEpisodeMinutes,
            TotalTrackedMinutes = trackedMovieMinutes + trackedEpisodeMinutes,
            RemainingMinutes = Math.Max(trackedMovieMinutes + trackedEpisodeMinutes - movieMinutes - watchedEpisodeMinutes, 0)
        };
    }

    public IReadOnlyList<SeriesStatistics> GetTopSeriesByWatchTime(IEnumerable<LibraryEntry> entries, int count)
    {
        return CalculateSeriesStatistics(entries)
            .OrderByDescending(stats => stats.WatchedMinutes)
            .ThenByDescending(stats => stats.CompletionPercent)
            .ThenBy(stats => stats.Title)
            .Take(Math.Max(count, 0))
            .ToList();
    }

    public IReadOnlyList<SeriesStatistics> GetMostEpisodesWatchedSeries(IEnumerable<LibraryEntry> entries, int count)
    {
        return CalculateSeriesStatistics(entries)
            .OrderByDescending(stats => stats.WatchedEpisodes)
            .ThenByDescending(stats => stats.WatchedMinutes)
            .ThenBy(stats => stats.Title)
            .Take(Math.Max(count, 0))
            .ToList();
    }

    public IReadOnlyList<SeriesStatistics> GetCurrentlyWatchingSeries(IEnumerable<LibraryEntry> entries, int count)
    {
        return CalculateSeriesStatistics(entries)
            .Where(stats => stats.Status == WatchStatus.InProgress)
            .OrderByDescending(stats => stats.WatchedEpisodes)
            .ThenByDescending(stats => stats.WatchedMinutes)
            .ThenBy(stats => stats.Title)
            .Take(Math.Max(count, 0))
            .ToList();
    }

    public IReadOnlyList<GenreWatchTimeStatistics> GetTopGenresByWatchTime(IEnumerable<LibraryEntry> entries, int count)
    {
        var genreTotals = new Dictionary<string, GenreWatchTimeAggregate>(StringComparer.OrdinalIgnoreCase);

        foreach (var entry in entries)
        {
            var genres = entry.Media.Genres
                .Where(genre => !string.IsNullOrWhiteSpace(genre))
                .Select(genre => genre.Trim())
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();

            if (genres.Count == 0)
            {
                continue;
            }

            var watchedMovieMinutes = entry.Media.Type == MediaType.Movie && entry.Movie?.IsWatched == true
                ? entry.Movie.DurationMinutes ?? 0
                : 0;
            var watchedEpisodeMinutes = entry.Media.Type == MediaType.Series
                ? CalculateSeriesStatistics(entry).WatchedMinutes
                : 0;

            if (watchedMovieMinutes <= 0 && watchedEpisodeMinutes <= 0)
            {
                continue;
            }

            foreach (var genre in genres)
            {
                if (!genreTotals.TryGetValue(genre, out var aggregate))
                {
                    aggregate = new GenreWatchTimeAggregate(genre);
                    genreTotals.Add(genre, aggregate);
                }

                aggregate.WatchedMovieMinutes += watchedMovieMinutes;
                aggregate.WatchedEpisodeMinutes += watchedEpisodeMinutes;
                aggregate.TitleIds.Add(entry.Media.Id);
            }
        }

        return genreTotals.Values
            .Select(aggregate => new GenreWatchTimeStatistics
            {
                Genre = aggregate.Genre,
                WatchedMovieMinutes = aggregate.WatchedMovieMinutes,
                WatchedEpisodeMinutes = aggregate.WatchedEpisodeMinutes,
                TotalWatchedMinutes = aggregate.WatchedMovieMinutes + aggregate.WatchedEpisodeMinutes,
                TitleCount = aggregate.TitleIds.Count
            })
            .OrderByDescending(stats => stats.TotalWatchedMinutes)
            .ThenByDescending(stats => stats.TitleCount)
            .ThenBy(stats => stats.Genre)
            .Take(Math.Max(count, 0))
            .ToList();
    }

    public IReadOnlyList<UniverseWatchTimeStatistics> GetTopUniversesByWatchTime(IEnumerable<LibraryEntry> entries, int count)
    {
        var universeTotals = new Dictionary<string, UniverseWatchTimeAggregate>(StringComparer.OrdinalIgnoreCase);

        foreach (var entry in entries)
        {
            var universe = GetUniverse(entry);
            if (string.IsNullOrWhiteSpace(universe))
            {
                continue;
            }

            var watchedMovieMinutes = entry.Media.Type == MediaType.Movie && entry.Movie?.IsWatched == true
                ? entry.Movie.DurationMinutes ?? 0
                : 0;
            var watchedEpisodeMinutes = entry.Media.Type == MediaType.Series
                ? CalculateSeriesStatistics(entry).WatchedMinutes
                : 0;

            if (watchedMovieMinutes <= 0 && watchedEpisodeMinutes <= 0)
            {
                continue;
            }

            if (!universeTotals.TryGetValue(universe, out var aggregate))
            {
                aggregate = new UniverseWatchTimeAggregate(universe);
                universeTotals.Add(universe, aggregate);
            }

            aggregate.WatchedMovieMinutes += watchedMovieMinutes;
            aggregate.WatchedEpisodeMinutes += watchedEpisodeMinutes;
            aggregate.TitleIds.Add(entry.Media.Id);
        }

        return universeTotals.Values
            .Select(aggregate => new UniverseWatchTimeStatistics
            {
                Universe = aggregate.Universe,
                WatchedMovieMinutes = aggregate.WatchedMovieMinutes,
                WatchedEpisodeMinutes = aggregate.WatchedEpisodeMinutes,
                TotalWatchedMinutes = aggregate.WatchedMovieMinutes + aggregate.WatchedEpisodeMinutes,
                TitleCount = aggregate.TitleIds.Count
            })
            .OrderByDescending(stats => stats.TotalWatchedMinutes)
            .ThenByDescending(stats => stats.TitleCount)
            .ThenBy(stats => stats.Universe)
            .Take(Math.Max(count, 0))
            .ToList();
    }

    public IReadOnlyList<SeasonStatistics> GetTopSeasonsByWatchTime(IEnumerable<LibraryEntry> entries, int count)
    {
        return CalculateSeriesStatistics(entries)
            .SelectMany(stats => stats.Seasons)
            .OrderByDescending(stats => stats.WatchedMinutes)
            .ThenByDescending(stats => stats.CompletionPercent)
            .ThenBy(stats => stats.SeriesTitle)
            .ThenBy(stats => stats.SeasonNumber)
            .Take(Math.Max(count, 0))
            .ToList();
    }

    private static SeasonStatistics CalculateSeasonStatistics(Season season, Guid seriesId, string seriesTitle)
    {
        var episodes = season.Episodes
            .OrderBy(episode => episode.EpisodeNumber)
            .ToList();

        var watchedEpisodes = episodes.Where(episode => episode.IsWatched).ToList();
        var totalEpisodes = episodes.Count;
        var watchedCount = watchedEpisodes.Count;
        var totalMinutes = episodes.Sum(episode => episode.DurationMinutes ?? 0);
        var watchedMinutes = watchedEpisodes.Sum(episode => episode.DurationMinutes ?? 0);

        return new SeasonStatistics
        {
            SeasonId = season.Id,
            SeriesId = seriesId,
            SeriesTitle = seriesTitle,
            SeasonNumber = season.SeasonNumber,
            Title = season.Title,
            TotalEpisodes = totalEpisodes,
            WatchedEpisodes = watchedCount,
            UnwatchedEpisodes = Math.Max(totalEpisodes - watchedCount, 0),
            TotalMinutes = totalMinutes,
            WatchedMinutes = watchedMinutes,
            RemainingMinutes = Math.Max(totalMinutes - watchedMinutes, 0),
            CompletionPercent = totalEpisodes == 0 ? 0 : watchedCount * 100d / totalEpisodes,
            Status = watchedCount switch
            {
                0 => WatchStatus.NotStarted,
                var count when count == totalEpisodes && totalEpisodes > 0 => WatchStatus.Completed,
                _ => WatchStatus.InProgress
            }
        };
    }

    private sealed class GenreWatchTimeAggregate(string genre)
    {
        public string Genre { get; } = genre;
        public int WatchedMovieMinutes { get; set; }
        public int WatchedEpisodeMinutes { get; set; }
        public HashSet<Guid> TitleIds { get; } = new();
    }

    private sealed class UniverseWatchTimeAggregate(string universe)
    {
        public string Universe { get; } = universe;
        public int WatchedMovieMinutes { get; set; }
        public int WatchedEpisodeMinutes { get; set; }
        public HashSet<Guid> TitleIds { get; } = new();
    }

    private static string? GetUniverse(LibraryEntry entry)
        => entry.Media.Type == MediaType.Movie
            ? NormalizeOptional(entry.Movie?.Universe)
            : NormalizeOptional(entry.Series?.Universe);

    private static string? NormalizeOptional(string? value)
        => string.IsNullOrWhiteSpace(value) ? null : value.Trim();
}
