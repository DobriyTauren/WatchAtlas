using WatchAtlas.Models.Enums;

namespace WatchAtlas.Models.Library;

public class LibraryFilterOptions
{
    public string SearchTerm { get; set; } = string.Empty;
    public string? Genre { get; set; }
    public string? Universe { get; set; }
    public MediaType? MediaType { get; set; }
    public WatchStatus? Status { get; set; }
    public LibrarySortBy SortBy { get; set; } = LibrarySortBy.UpdatedAt;
    public bool Descending { get; set; } = true;
}
