using System.Globalization;
using Microsoft.JSInterop;
using WatchAtlas.Helpers;
using WatchAtlas.Models.Enums;

namespace WatchAtlas.State;

public class LocalizationState(
    IJSRuntime jsRuntime,
    SettingsState settingsState) : StateStoreBase
{
    public AppLanguage CurrentLanguage { get; private set; } = AppLanguage.English;
    public bool IsInitialized { get; private set; }

    public string this[string text] => LocalizedText.Translate(text);

    public string Format(string text, params object?[] args)
        => LocalizedText.Format(text, args);

    public async Task InitializeAsync(CancellationToken cancellationToken = default)
    {
        if (IsInitialized)
        {
            return;
        }

        await RefreshFromSettingsAsync(cancellationToken);

        IsInitialized = true;
        NotifyStateChanged();
    }

    public Task SetLanguageAsync(AppLanguage language, CancellationToken cancellationToken = default)
        => ApplyLanguageAsync(language, persist: true, cancellationToken);

    public async Task RefreshFromSettingsAsync(CancellationToken cancellationToken = default)
    {
        await settingsState.EnsureLoadedAsync(cancellationToken);

        var language = settingsState.Current.Language ?? await DetectAndPersistLanguageAsync(cancellationToken);
        await ApplyLanguageAsync(language, persist: false, cancellationToken);
    }

    private async Task<AppLanguage> DetectAndPersistLanguageAsync(CancellationToken cancellationToken)
    {
        string? browserLanguage = null;

        try
        {
            browserLanguage = await jsRuntime.InvokeAsync<string?>("watchAtlasStorage.getPreferredLanguage");
        }
        catch (JSException)
        {
        }

        var detectedLanguage = browserLanguage?.StartsWith("ru", StringComparison.OrdinalIgnoreCase) == true
            ? AppLanguage.Russian
            : AppLanguage.English;

        await settingsState.UpdateLanguageAsync(detectedLanguage, cancellationToken);
        return detectedLanguage;
    }

    private async Task ApplyLanguageAsync(AppLanguage language, bool persist, CancellationToken cancellationToken)
    {
        if (persist)
        {
            await settingsState.UpdateLanguageAsync(language, cancellationToken);
        }

        var culture = new CultureInfo(language == AppLanguage.Russian ? "ru-RU" : "en-US");
        CultureInfo.DefaultThreadCurrentCulture = culture;
        CultureInfo.DefaultThreadCurrentUICulture = culture;
        CultureInfo.CurrentCulture = culture;
        CultureInfo.CurrentUICulture = culture;

        CurrentLanguage = language;

        try
        {
            await jsRuntime.InvokeVoidAsync("watchAtlasStorage.setDocumentLanguage", culture.TwoLetterISOLanguageName);
        }
        catch (JSException)
        {
        }

        NotifyStateChanged();
    }
}
