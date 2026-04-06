using System.Text.Json;
using WatchAtlas.Helpers;
using WatchAtlas.Models.Library;

namespace WatchAtlas.Services;

public class ImportExportService : IImportExportService
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web)
    {
        WriteIndented = true
    };

    public string Export(LibraryStorageModel storage)
    {
        var snapshot = LibraryCloneHelper.Clone(storage);
        snapshot.SchemaVersion = LibraryStorageModel.CurrentSchemaVersion;

        return JsonSerializer.Serialize(snapshot, JsonOptions);
    }

    public LibraryStorageModel Import(string json)
        => LibraryStorageMigrationHelper.Deserialize(json, JsonOptions);
}
