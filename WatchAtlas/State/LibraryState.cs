using WatchAtlas.Models.Library;
using WatchAtlas.Repositories;

namespace WatchAtlas.State;

public class LibraryState(IMediaRepository repository) : StateStoreBase
{
    public IReadOnlyList<LibraryEntry> Entries { get; private set; } = Array.Empty<LibraryEntry>();
    public bool IsLoaded { get; private set; }
    public bool IsLoading { get; private set; }

    public async Task EnsureLoadedAsync(CancellationToken cancellationToken = default)
    {
        if (IsLoaded || IsLoading)
        {
            return;
        }

        IsLoading = true;
        NotifyStateChanged();

        Entries = await repository.GetAllAsync(cancellationToken);
        IsLoaded = true;
        IsLoading = false;
        NotifyStateChanged();
    }

    public async Task RefreshAsync(CancellationToken cancellationToken = default)
    {
        Entries = await repository.GetAllAsync(cancellationToken);
        IsLoaded = true;
        IsLoading = false;
        NotifyStateChanged();
    }

    public LibraryEntry? GetById(Guid id) => Entries.FirstOrDefault(entry => entry.Media.Id == id);
}
