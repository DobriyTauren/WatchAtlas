using WatchAtlas.Models.Enums;

namespace WatchAtlas.Models;

public class AppSettings
{
    public ThemeMode ThemeMode { get; set; } = ThemeMode.LightSoft;
    public LibraryViewMode LibraryViewMode { get; set; } = LibraryViewMode.Grid;
    public LibrarySortBy DefaultLibrarySortBy { get; set; } = LibrarySortBy.UpdatedAt;
    public bool DefaultLibrarySortDescending { get; set; } = true;
    public bool UseDenseLibraryGrid { get; set; }
    public bool ShowCompletedItemsFirst { get; set; }

    public static AppSettings Default => new();
}
