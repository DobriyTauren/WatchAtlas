using WatchAtlas.Models;
using WatchAtlas.Models.Library;

namespace WatchAtlas.Services;

public interface IStatisticsService
{
    GlobalStatistics CalculateGlobalStatistics(IEnumerable<LibraryEntry> entries);
    IReadOnlyList<SeriesStatistics> CalculateSeriesStatistics(IEnumerable<LibraryEntry> entries);
    SeriesProgressSummary CalculateSeriesProgress(LibraryEntry entry);
    SeriesStatistics CalculateSeriesStatistics(LibraryEntry entry);
    SeasonStatistics CalculateSeasonStatistics(Season season);
    WatchTimeSummary CalculateWatchTimeSummary(IEnumerable<LibraryEntry> entries);
    IReadOnlyList<SeriesStatistics> GetTopSeriesByWatchTime(IEnumerable<LibraryEntry> entries, int count);
    IReadOnlyList<SeriesStatistics> GetMostCompletedSeries(IEnumerable<LibraryEntry> entries, int count);
    IReadOnlyList<SeasonStatistics> GetTopSeasonsByWatchTime(IEnumerable<LibraryEntry> entries, int count);
}
