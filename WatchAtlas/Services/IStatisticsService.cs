using WatchAtlas.Models;
using WatchAtlas.Models.Library;

namespace WatchAtlas.Services;

public interface IStatisticsService
{
    GlobalStatistics CalculateGlobalStatistics(IEnumerable<LibraryEntry> entries);
    SeriesProgressSummary CalculateSeriesProgress(LibraryEntry entry);
    SeriesStatistics CalculateSeriesStatistics(LibraryEntry entry);
    SeasonStatistics CalculateSeasonStatistics(Season season);
}
