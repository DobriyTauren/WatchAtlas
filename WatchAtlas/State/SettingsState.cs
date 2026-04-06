using WatchAtlas.Models;
using WatchAtlas.Models.Enums;
using WatchAtlas.Repositories;

namespace WatchAtlas.State;

public class SettingsState(ISettingsRepository repository) : StateStoreBase
{
    public AppSettings Current { get; private set; } = AppSettings.Default;
    public bool IsLoaded { get; private set; }

    public async Task EnsureLoadedAsync(CancellationToken cancellationToken = default)
    {
        if (IsLoaded)
        {
            return;
        }

        Current = await repository.GetAsync(cancellationToken);
        IsLoaded = true;
        NotifyStateChanged();
    }

    public async Task UpdateThemeAsync(ThemeMode themeMode, CancellationToken cancellationToken = default)
    {
        await EnsureLoadedAsync(cancellationToken);
        Current.ThemeMode = themeMode;
        await repository.SaveAsync(Current, cancellationToken);
        NotifyStateChanged();
    }
}
