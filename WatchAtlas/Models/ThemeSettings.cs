using WatchAtlas.Models.Enums;

namespace WatchAtlas.Models;

public class ThemeSettings
{
    public required ThemeMode Mode { get; init; }
    public required string DisplayName { get; init; }
    public required string Description { get; init; }
    public required string AccentLabel { get; init; }
    public required string ThemeKey { get; init; }
}
