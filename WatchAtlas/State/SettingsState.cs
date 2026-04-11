using WatchAtlas.Helpers;
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

    public async Task UpdateLanguageAsync(AppLanguage language, CancellationToken cancellationToken = default)
    {
        await EnsureLoadedAsync(cancellationToken);
        if (Current.Language == language)
        {
            return;
        }

        Current.Language = language;
        await repository.SaveAsync(Current, cancellationToken);
        NotifyStateChanged();
    }

    public async Task UpdateLibraryViewModeAsync(LibraryViewMode viewMode, CancellationToken cancellationToken = default)
    {
        await EnsureLoadedAsync(cancellationToken);
        if (Current.LibraryViewMode == viewMode)
        {
            return;
        }

        Current.LibraryViewMode = viewMode;
        await repository.SaveAsync(Current, cancellationToken);
        NotifyStateChanged();
    }

    public async Task UpdateDefaultSortAsync(
        LibrarySortBy sortBy,
        bool descending,
        CancellationToken cancellationToken = default)
    {
        await EnsureLoadedAsync(cancellationToken);
        if (Current.DefaultLibrarySortBy == sortBy &&
            Current.DefaultLibrarySortDescending == descending)
        {
            return;
        }

        Current.DefaultLibrarySortBy = sortBy;
        Current.DefaultLibrarySortDescending = descending;
        await repository.SaveAsync(Current, cancellationToken);
        NotifyStateChanged();
    }

    public async Task ReplaceAsync(AppSettings settings, CancellationToken cancellationToken = default)
    {
        Current = LibraryCloneHelper.Clone(settings);
        IsLoaded = true;
        await repository.SaveAsync(Current, cancellationToken);
        NotifyStateChanged();
    }

    public async Task ResetAsync(CancellationToken cancellationToken = default)
    {
        await repository.ResetAsync(cancellationToken);
        Current = AppSettings.Default;
        IsLoaded = true;
        NotifyStateChanged();
    }
}
