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

    public void Reset()
    {
        Options.SearchTerm = string.Empty;
        Options.MediaType = null;
        Options.Status = null;
        Options.SeriesId = null;
        Options.SortBy = LibrarySortBy.RecentlyUpdated;
        Options.Descending = true;
        NotifyStateChanged();
    }

    public LibraryQueryResult Apply(IEnumerable<LibraryEntry> entries)
    {
        var sourceEntries = entries.ToList();
        IEnumerable<LibraryEntry> query = sourceEntries;

        if (!string.IsNullOrWhiteSpace(Options.SearchTerm))
        {
            query = query.Where(entry => entry.Media.Title.Contains(Options.SearchTerm, StringComparison.OrdinalIgnoreCase));
        }

        if (Options.MediaType is not null)
        {
            query = query.Where(entry => entry.Media.Type == Options.MediaType);
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
