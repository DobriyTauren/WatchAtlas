using WatchAtlas.Models.Library;

namespace WatchAtlas.Repositories;

public interface IMediaRepository
{
    Task<IReadOnlyList<LibraryEntry>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<LibraryEntry?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task SaveAsync(LibraryEntry entry, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task ResetAsync(CancellationToken cancellationToken = default);
}
