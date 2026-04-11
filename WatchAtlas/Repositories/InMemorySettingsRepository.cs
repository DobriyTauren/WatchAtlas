using System.Text.Json;
using Microsoft.JSInterop;
using WatchAtlas.Models;

namespace WatchAtlas.Repositories;

public class LocalStorageSettingsRepository(IJSRuntime jsRuntime) : ISettingsRepository
{
    private const string StorageKey = "watchatlas.settings";
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);
    private AppSettings? _settings;

    public async Task<AppSettings> GetAsync(CancellationToken cancellationToken = default)
    {
        if (_settings is null)
        {
            var json = await jsRuntime.InvokeAsync<string?>("watchAtlasStorage.getItem", StorageKey);
            _settings = Deserialize(json);
        }

        return Clone(_settings);
    }

    public async Task SaveAsync(AppSettings settings, CancellationToken cancellationToken = default)
    {
        _settings = Clone(settings);
        var json = JsonSerializer.Serialize(_settings, JsonOptions);
        await jsRuntime.InvokeVoidAsync("watchAtlasStorage.setItem", StorageKey, json);
    }

    public async Task ResetAsync(CancellationToken cancellationToken = default)
    {
        _settings = AppSettings.Default;
        await jsRuntime.InvokeVoidAsync("watchAtlasStorage.removeItem", StorageKey);
    }

    private static AppSettings Deserialize(string? json)
    {
        if (string.IsNullOrWhiteSpace(json))
        {
            return AppSettings.Default;
        }

        try
        {
            return JsonSerializer.Deserialize<AppSettings>(json, JsonOptions) ?? AppSettings.Default;
        }
        catch (JsonException)
        {
            return AppSettings.Default;
        }
    }

    private static AppSettings Clone(AppSettings settings) => new()
    {
        ThemeMode = settings.ThemeMode,
        Language = settings.Language,
        LibraryViewMode = settings.LibraryViewMode,
        DefaultLibrarySortBy = settings.DefaultLibrarySortBy,
        DefaultLibrarySortDescending = settings.DefaultLibrarySortDescending
    };
}
