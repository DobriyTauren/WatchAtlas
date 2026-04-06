using System.Text.Json;
using WatchAtlas.Models.Library;

namespace WatchAtlas.Helpers;

public static class LibraryStorageMigrationHelper
{
    public static LibraryStorageModel Deserialize(string? json, JsonSerializerOptions options)
    {
        if (string.IsNullOrWhiteSpace(json))
        {
            return LibraryStorageModel.CreateDefault();
        }

        try
        {
            using var document = JsonDocument.Parse(json);
            return Migrate(document.RootElement, options);
        }
        catch (JsonException)
        {
            return LibraryStorageModel.CreateDefault();
        }
    }

    private static LibraryStorageModel Migrate(JsonElement root, JsonSerializerOptions options)
    {
        if (root.ValueKind == JsonValueKind.Array)
        {
            return MigrateLegacyEntries(root, options);
        }

        if (root.ValueKind != JsonValueKind.Object)
        {
            return LibraryStorageModel.CreateDefault();
        }

        if (root.TryGetProperty("entries", out var entriesElement) && entriesElement.ValueKind == JsonValueKind.Array)
        {
            return MigrateLegacyEntries(entriesElement, options);
        }

        var storage = root.Deserialize<LibraryStorageModel>(options) ?? LibraryStorageModel.CreateDefault();
        storage.SchemaVersion = LibraryStorageModel.CurrentSchemaVersion;
        storage.MediaItems ??= new();
        storage.Movies ??= new();
        storage.Series ??= new();

        return storage;
    }

    private static LibraryStorageModel MigrateLegacyEntries(JsonElement entriesElement, JsonSerializerOptions options)
    {
        var entries = entriesElement.Deserialize<List<LibraryEntry>>(options) ?? new();

        return new LibraryStorageModel
        {
            SchemaVersion = LibraryStorageModel.CurrentSchemaVersion,
            MediaItems = entries.Select(entry => LibraryCloneHelper.Clone(entry.Media)).ToList(),
            Movies = entries
                .Where(entry => entry.Movie is not null)
                .Select(entry => LibraryCloneHelper.Clone(entry.Movie)!)
                .ToList(),
            Series = entries
                .Where(entry => entry.Series is not null)
                .Select(entry => LibraryCloneHelper.Clone(entry.Series)!)
                .ToList()
        };
    }
}
