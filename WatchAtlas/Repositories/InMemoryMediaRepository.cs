using WatchAtlas.Models.Library;

namespace WatchAtlas.Repositories;

public class InMemoryMediaRepository : IMediaRepository
{
    private readonly List<LibraryEntry> _entries = new();

    public Task<IReadOnlyList<LibraryEntry>> GetAllAsync(CancellationToken cancellationToken = default)
        => Task.FromResult<IReadOnlyList<LibraryEntry>>(_entries.OrderByDescending(entry => entry.Media.UpdatedAt).ToList());

    public Task<LibraryEntry?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => Task.FromResult(_entries.FirstOrDefault(entry => entry.Media.Id == id));

    public Task SaveAsync(LibraryEntry entry, CancellationToken cancellationToken = default)
    {
        var existingIndex = _entries.FindIndex(candidate => candidate.Media.Id == entry.Media.Id);
        if (existingIndex >= 0)
        {
            _entries[existingIndex] = entry;
        }
        else
        {
            _entries.Add(entry);
        }

        return Task.CompletedTask;
    }

    public Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        _entries.RemoveAll(entry => entry.Media.Id == id);
        return Task.CompletedTask;
    }

    public Task ResetAsync(CancellationToken cancellationToken = default)
    {
        _entries.Clear();
        return Task.CompletedTask;
    }
}
