using System.Text.Json;
using WatchAtlas.Models;
using WatchAtlas.Models.Enums;
using WatchAtlas.Repositories;
using WatchAtlas.Tests.Support;
using Xunit;

namespace WatchAtlas.Tests.Repositories;

public class LocalStorageSettingsRepositoryTests
{
    [Fact]
    public async Task GetAsync_WithCorruptedStorage_ReturnsDefaultSettings()
    {
        var jsRuntime = new FakeJsRuntime();
        jsRuntime.Seed("watchatlas.settings", "{ invalid");
        var repository = new LocalStorageSettingsRepository(jsRuntime);

        var settings = await repository.GetAsync();

        Assert.Equal(AppSettings.Default.ThemeMode, settings.ThemeMode);
        Assert.Equal(AppSettings.Default.LibraryViewMode, settings.LibraryViewMode);
        Assert.Equal(AppSettings.Default.DefaultLibrarySortBy, settings.DefaultLibrarySortBy);
    }

    [Fact]
    public async Task SaveAsync_StoresClonedSettingsSnapshot()
    {
        var jsRuntime = new FakeJsRuntime();
        var repository = new LocalStorageSettingsRepository(jsRuntime);
        var settings = new AppSettings
        {
            ThemeMode = ThemeMode.DarkSoft,
            LibraryViewMode = LibraryViewMode.List,
            DefaultLibrarySortBy = LibrarySortBy.Progress,
            DefaultLibrarySortDescending = false,
        };

        await repository.SaveAsync(settings);
        settings.ThemeMode = ThemeMode.LightSoft;

        var reloaded = await repository.GetAsync();
        var rawJson = jsRuntime.GetStoredValue("watchatlas.settings");
        using var document = JsonDocument.Parse(rawJson!);

        Assert.Equal(ThemeMode.DarkSoft, reloaded.ThemeMode);
        Assert.Equal(LibraryViewMode.List, reloaded.LibraryViewMode);
        Assert.Equal((int)LibrarySortBy.Progress, document.RootElement.GetProperty("defaultLibrarySortBy").GetInt32());
    }
}
