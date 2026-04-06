using WatchAtlas.Helpers;
using WatchAtlas.Models.Enums;
using WatchAtlas.Models.Library;

namespace WatchAtlas.State;

public class FilterState : StateStoreBase
{
    public LibraryFilterOptions Options { get; } = new();

    public void SetSearchTerm(string value)
    {
        if (Options.SearchTerm == value)
        {
            return;
        }

        Options.SearchTerm = value;
        NotifyStateChanged();
    }

    public void SetMediaType(MediaType? value)
    {
        if (Options.MediaType == value)
        {
            return;
        }

        Options.MediaType = value;
        NotifyStateChanged();
    }

    public void SetGenre(string? value)
    {
        var normalized = string.IsNullOrWhiteSpace(value) ? null : value.Trim();
        if (Options.Genre == normalized)
        {
            return;
        }

        Options.Genre = normalized;
        NotifyStateChanged();
    }

    public void SetStatus(WatchStatus? value)
    {
        if (Options.Status == value)
        {
            return;
        }

        Options.Status = value;
        NotifyStateChanged();
    }

    public void SetSeriesId(Guid? value)
    {
        if (Options.SeriesId == value)
        {
            return;
        }

        Options.SeriesId = value;
        NotifyStateChanged();
    }

    public void SetSortBy(LibrarySortBy value)
    {
        if (Options.SortBy == value)
        {
            return;
        }

        Options.SortBy = value;
        NotifyStateChanged();
    }

    public void SetDescending(bool value)
    {
        if (Options.Descending == value)
        {
            return;
        }

        Options.Descending = value;
        NotifyStateChanged();
    }

    public void Reset()
    {
        Options.SearchTerm = string.Empty;
        Options.Genre = null;
        Options.MediaType = null;
        Options.Status = null;
        Options.SeriesId = null;
        Options.SortBy = LibrarySortBy.UpdatedAt;
        Options.Descending = true;
        NotifyStateChanged();
    }

    public LibraryQueryResult Apply(IEnumerable<LibraryEntry> entries)
    {
        var sourceEntries = entries.ToList();
        IEnumerable<LibraryEntry> query = sourceEntries;

        if (!string.IsNullOrWhiteSpace(Options.SearchTerm))
        {
            query = query.Where(entry =>
                entry.Media.Title.Contains(Options.SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                (entry.Media.Description?.Contains(Options.SearchTerm, StringComparison.OrdinalIgnoreCase) ?? false) ||
                entry.Media.Genres.Any(genre => genre.Contains(Options.SearchTerm, StringComparison.OrdinalIgnoreCase)));
        }

        if (Options.MediaType is not null)
        {
            query = query.Where(entry => entry.Media.Type == Options.MediaType);
        }

        if (!string.IsNullOrWhiteSpace(Options.Genre))
        {
            query = query.Where(entry => entry.Media.Genres.Any(genre => genre.Equals(Options.Genre, StringComparison.OrdinalIgnoreCase)));
        }

        if (Options.Status is not null)
        {
            query = query.Where(entry => WatchStatusHelper.GetStatus(entry) == Options.Status);
        }

        if (Options.SeriesId is not null)
        {
            query = query.Where(entry => entry.Media.Id == Options.SeriesId.Value);
        }

        query = Options.SortBy switch
        {
            LibrarySortBy.Title => Options.Descending
                ? query.OrderByDescending(entry => entry.Media.Title)
                : query.OrderBy(entry => entry.Media.Title),
            LibrarySortBy.Rating => Options.Descending
                ? query.OrderByDescending(entry => entry.Media.PersonalRating ?? 0)
                : query.OrderBy(entry => entry.Media.PersonalRating ?? 0),
            LibrarySortBy.CreatedAt => Options.Descending
                ? query.OrderByDescending(entry => entry.Media.CreatedAt)
                : query.OrderBy(entry => entry.Media.CreatedAt),
            LibrarySortBy.Progress => Options.Descending
                ? query.OrderByDescending(WatchStatusHelper.GetCompletionPercent)
                    .ThenByDescending(entry => entry.Media.UpdatedAt)
                : query.OrderBy(WatchStatusHelper.GetCompletionPercent)
                    .ThenBy(entry => entry.Media.UpdatedAt),
            _ => Options.Descending
                ? query.OrderByDescending(entry => entry.Media.UpdatedAt)
                : query.OrderBy(entry => entry.Media.UpdatedAt)
        };

        var items = query.ToList();

        return new LibraryQueryResult
        {
            Items = items,
            Filters = new LibraryFilterOptions
            {
                SearchTerm = Options.SearchTerm,
                Genre = Options.Genre,
                MediaType = Options.MediaType,
                Status = Options.Status,
                SeriesId = Options.SeriesId,
                SortBy = Options.SortBy,
                Descending = Options.Descending
            },
            TotalCount = sourceEntries.Count,
            FilteredCount = items.Count
        };
    }
}
