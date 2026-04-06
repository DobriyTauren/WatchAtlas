using WatchAtlas.Models.Enums;

namespace WatchAtlas.Helpers;

public static class LibrarySortOptionHelper
{
    public static IReadOnlyList<LibrarySortOption> Options { get; } =
    [
        new("updated-desc", "Recently updated", LibrarySortBy.UpdatedAt, true),
        new("updated-asc", "Least recently updated", LibrarySortBy.UpdatedAt, false),
        new("created-desc", "Recently added", LibrarySortBy.CreatedAt, true),
        new("created-asc", "Oldest first", LibrarySortBy.CreatedAt, false),
        new("title-asc", "Title A-Z", LibrarySortBy.Title, false),
        new("title-desc", "Title Z-A", LibrarySortBy.Title, true),
        new("rating-desc", "Highest rating", LibrarySortBy.Rating, true),
        new("rating-asc", "Lowest rating", LibrarySortBy.Rating, false),
        new("progress-desc", "Most progress", LibrarySortBy.Progress, true),
        new("progress-asc", "Least progress", LibrarySortBy.Progress, false)
    ];

    public static string ToKey(LibrarySortBy sortBy, bool descending)
    {
        var option = Options.FirstOrDefault(item => item.SortBy == sortBy && item.Descending == descending);
        return option?.Key ?? "updated-desc";
    }

    public static LibrarySortOption Parse(string key)
        => Options.FirstOrDefault(option => option.Key.Equals(key, StringComparison.OrdinalIgnoreCase))
            ?? Options[0];
}

public record LibrarySortOption(string Key, string Label, LibrarySortBy SortBy, bool Descending);
