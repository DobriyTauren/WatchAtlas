using WatchAtlas.Models;
using WatchAtlas.Models.Enums;
using WatchAtlas.Services;

namespace WatchAtlas.State;

public class ThemeState(
    IThemeService themeService,
    SettingsState settingsState) : StateStoreBase
{
    private bool _isInitialized;

    public IReadOnlyList<ThemeSettings> AvailableThemes => themeService.GetAvailableThemes();
    public ThemeSettings CurrentTheme { get; private set; } = themeService.GetTheme(ThemeMode.LightSoft);

    public async Task InitializeAsync(CancellationToken cancellationToken = default)
    {
        if (_isInitialized)
        {
            return;
        }

        await settingsState.EnsureLoadedAsync(cancellationToken);
        CurrentTheme = await themeService.InitializeAsync(cancellationToken);
        settingsState.Current.ThemeMode = CurrentTheme.Mode;
        _isInitialized = true;
        NotifyStateChanged();
    }

    public async Task ApplyThemeAsync(ThemeMode mode, CancellationToken cancellationToken = default)
    {
        CurrentTheme = await themeService.ApplyThemeAsync(mode, cancellationToken);
        settingsState.Current.ThemeMode = CurrentTheme.Mode;
        NotifyStateChanged();
    }
}
