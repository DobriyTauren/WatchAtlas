using Microsoft.JSInterop;
using WatchAtlas.Models;
using WatchAtlas.Models.Enums;
using WatchAtlas.Repositories;

namespace WatchAtlas.Services;

public class ThemeService(
    ISettingsRepository settingsRepository,
    IJSRuntime jsRuntime) : IThemeService
{
    private static readonly IReadOnlyList<ThemeSettings> Themes = new List<ThemeSettings>
    {
        new()
        {
            Mode = ThemeMode.LightSoft,
            DisplayName = "Light Soft",
            Description = "Warm porcelain surfaces with rosy accents for everyday browsing.",
            AccentLabel = "Rose cloud",
            ThemeKey = "light-soft",
            AccentHex = "#d17187",
            SurfaceHex = "#fffaf7",
            HighlightHex = "#c9def8"
        },
        new()
        {
            Mode = ThemeMode.DarkSoft,
            DisplayName = "Dark Soft",
            Description = "Charcoal layers with cool blue highlights and comfortable contrast.",
            AccentLabel = "Moonlit blue",
            ThemeKey = "dark-soft",
            AccentHex = "#84abff",
            SurfaceHex = "#1d2434",
            HighlightHex = "#4095c6"
        },
        new()
        {
            Mode = ThemeMode.Pastel,
            DisplayName = "Pastel",
            Description = "Lavender and blush surfaces for a softer collector-style library.",
            AccentLabel = "Lavender mist",
            ThemeKey = "pastel",
            AccentHex = "#997de0",
            SurfaceHex = "#fdf7ff",
            HighlightHex = "#ffd7e9"
        }
    };

    public IReadOnlyList<ThemeSettings> GetAvailableThemes() => Themes;

    public ThemeSettings GetTheme(ThemeMode mode) => Themes.First(theme => theme.Mode == mode);

    public async Task<ThemeSettings> InitializeAsync(CancellationToken cancellationToken = default)
    {
        var settings = await settingsRepository.GetAsync(cancellationToken);
        var theme = GetTheme(settings.ThemeMode);

        await ApplyThemeToDocumentAsync(theme);
        return theme;
    }

    public async Task<ThemeSettings> ApplyThemeAsync(ThemeMode mode, CancellationToken cancellationToken = default)
    {
        var settings = await settingsRepository.GetAsync(cancellationToken);
        settings.ThemeMode = mode;
        await settingsRepository.SaveAsync(settings, cancellationToken);

        var theme = GetTheme(mode);
        await ApplyThemeToDocumentAsync(theme);

        return theme;
    }

    private ValueTask ApplyThemeToDocumentAsync(ThemeSettings theme)
        => jsRuntime.InvokeVoidAsync("watchAtlasTheme.apply", theme.ThemeKey, theme.AccentHex);
}
