using WatchAtlas.Models.Enums;

namespace WatchAtlas.Models;

public class AppSettings
{
    public ThemeMode ThemeMode { get; set; } = ThemeMode.LightSoft;
    public AppLanguage? Language { get; set; }
    public LibraryViewMode LibraryViewMode { get; set; } = LibraryViewMode.Grid;
    public LibrarySortBy DefaultLibrarySortBy { get; set; } = LibrarySortBy.UpdatedAt;
    public bool DefaultLibrarySortDescending { get; set; } = true;

    public static AppSettings Default => new();
}
