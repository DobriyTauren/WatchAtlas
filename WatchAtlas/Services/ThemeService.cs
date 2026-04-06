using WatchAtlas.Models;
using WatchAtlas.Models.Enums;

namespace WatchAtlas.Services;

public class ThemeService : IThemeService
{
    private static readonly IReadOnlyList<ThemeSettings> Themes = new List<ThemeSettings>
    {
        new()
        {
            Mode = ThemeMode.LightSoft,
            DisplayName = "Light Soft",
            Description = "Warm porcelain surfaces with rosy accents for everyday browsing.",
            AccentLabel = "Rose cloud",
            ThemeKey = "light-soft"
        },
        new()
        {
            Mode = ThemeMode.DarkSoft,
            DisplayName = "Dark Soft",
            Description = "Charcoal layers with cool blue highlights and comfortable contrast.",
            AccentLabel = "Moonlit blue",
            ThemeKey = "dark-soft"
        },
        new()
        {
            Mode = ThemeMode.Pastel,
            DisplayName = "Pastel",
            Description = "Lavender and blush surfaces for a softer collector-style library.",
            AccentLabel = "Lavender mist",
            ThemeKey = "pastel"
        }
    };

    public IReadOnlyList<ThemeSettings> GetAvailableThemes() => Themes;

    public ThemeSettings GetTheme(ThemeMode mode) => Themes.First(theme => theme.Mode == mode);
}
