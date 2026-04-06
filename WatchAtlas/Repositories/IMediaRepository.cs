using WatchAtlas.Models;

namespace WatchAtlas.Repositories;

public interface IMediaRepository
{
    Task<List<MediaItem>> GetAllAsync();
    Task<MediaItem?> GetByIdAsync(Guid id);
    Task AddAsync(MediaItem item);
    Task UpdateAsync(MediaItem item);
    Task DeleteAsync(Guid id);
    Task<MovieDetails?> GetMovieDetailsAsync(Guid mediaItemId);
    Task SaveMovieDetailsAsync(MovieDetails details);
    Task<SeriesDetails?> GetSeriesDetailsAsync(Guid mediaItemId);
    Task SaveSeriesDetailsAsync(SeriesDetails details);
    Task ResetAsync();
}
