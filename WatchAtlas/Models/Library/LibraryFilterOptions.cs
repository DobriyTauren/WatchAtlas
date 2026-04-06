using WatchAtlas.Models.Enums;

namespace WatchAtlas.Models.Library;

public class LibraryFilterOptions
{
    public string SearchTerm { get; set; } = string.Empty;
    public MediaType? MediaType { get; set; }
    public WatchStatus? Status { get; set; }
    public Guid? SeriesId { get; set; }
    public LibrarySortBy SortBy { get; set; } = LibrarySortBy.RecentlyUpdated;
    public bool Descending { get; set; } = true;
}
