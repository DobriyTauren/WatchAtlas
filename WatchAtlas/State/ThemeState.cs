using Microsoft.JSInterop;
using WatchAtlas.Models;
using WatchAtlas.Models.Enums;
using WatchAtlas.Services;

namespace WatchAtlas.State;

public class ThemeState(
    IThemeService themeService,
    SettingsState settingsState,
    IJSRuntime jsRuntime) : StateStoreBase
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
        CurrentTheme = themeService.GetTheme(settingsState.Current.ThemeMode);
        await ApplyToBrowserAsync();
        _isInitialized = true;
        NotifyStateChanged();
    }

    public async Task ApplyThemeAsync(ThemeMode mode, CancellationToken cancellationToken = default)
    {
        CurrentTheme = themeService.GetTheme(mode);
        await settingsState.UpdateThemeAsync(mode, cancellationToken);
        await ApplyToBrowserAsync();
        NotifyStateChanged();
    }

    private ValueTask ApplyToBrowserAsync()
        => jsRuntime.InvokeVoidAsync("watchAtlasTheme.apply", CurrentTheme.ThemeKey);
}
