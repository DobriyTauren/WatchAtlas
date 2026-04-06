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

    public void SetSortBy(LibrarySortBy value)
    {
        if (Options.SortBy == value)
        {
            return;
        }

        Options.SortBy = value;
        NotifyStateChanged();
    }

    public IEnumerable<LibraryEntry> Apply(IEnumerable<LibraryEntry> entries)
    {
        var query = entries;

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

        return query.ToList();
    }
}
