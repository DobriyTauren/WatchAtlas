using WatchAtlas.Models;

namespace WatchAtlas.Repositories;

public class InMemorySettingsRepository : ISettingsRepository
{
    private AppSettings _settings = AppSettings.Default;

    public Task<AppSettings> GetAsync(CancellationToken cancellationToken = default)
        => Task.FromResult(new AppSettings
        {
            ThemeMode = _settings.ThemeMode,
            UseDenseLibraryGrid = _settings.UseDenseLibraryGrid,
            ShowCompletedItemsFirst = _settings.ShowCompletedItemsFirst
        });

    public Task SaveAsync(AppSettings settings, CancellationToken cancellationToken = default)
    {
        _settings = new AppSettings
        {
            ThemeMode = settings.ThemeMode,
            UseDenseLibraryGrid = settings.UseDenseLibraryGrid,
            ShowCompletedItemsFirst = settings.ShowCompletedItemsFirst
        };

        return Task.CompletedTask;
    }
}
