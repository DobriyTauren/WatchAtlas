using WatchAtlas.Models;
using WatchAtlas.Models.Enums;

namespace WatchAtlas.Services;

public interface IThemeService
{
    IReadOnlyList<ThemeSettings> GetAvailableThemes();
    ThemeSettings GetTheme(ThemeMode mode);
}
