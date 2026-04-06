namespace WatchAtlas.Models.Library;

public class LibraryQueryResult
{
    public required IReadOnlyList<LibraryEntry> Items { get; init; }
    public required LibraryFilterOptions Filters { get; init; }
    public int TotalCount { get; init; }
    public int FilteredCount { get; init; }
}
