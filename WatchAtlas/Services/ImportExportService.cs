using System.Text.Json;
using WatchAtlas.Helpers;
using WatchAtlas.Models;
using WatchAtlas.Models.Library;

namespace WatchAtlas.Services;

public class ImportExportService : IImportExportService
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web)
    {
        WriteIndented = true
    };

    public string Export(LibraryBackupModel backup)
    {
        var snapshot = LibraryCloneHelper.Clone(backup);
        snapshot.BackupVersion = LibraryBackupModel.CurrentBackupVersion;
        snapshot.ExportedAtUtc = DateTime.UtcNow;
        snapshot.Library.SchemaVersion = LibraryStorageModel.CurrentSchemaVersion;

        return JsonSerializer.Serialize(snapshot, JsonOptions);
    }

    public LibraryImportResult Import(string json)
    {
        if (string.IsNullOrWhiteSpace(json))
        {
            return CreateFailure(LocalizedText.Translate("The selected backup file is empty."));
        }

        try
        {
            using var document = JsonDocument.Parse(json);
            var root = document.RootElement;

            if (LooksLikeBackup(root))
            {
                var backup = root.Deserialize<LibraryBackupModel>(JsonOptions);
                if (backup is null)
                {
            return CreateFailure(LocalizedText.Translate("The selected backup file could not be read."));
                }

                return CreateSuccess(NormalizeBackup(backup));
            }

            if (LooksLikeLibrary(root))
            {
                var library = LibraryStorageMigrationHelper.Deserialize(json, JsonOptions);
                return CreateSuccess(new LibraryBackupModel
                {
                    BackupVersion = LibraryBackupModel.CurrentBackupVersion,
                    ExportedAtUtc = DateTime.UtcNow,
                    Library = library,
                    Settings = AppSettings.Default
                });
            }

            return CreateFailure(LocalizedText.Translate("The selected file does not match the WatchAtlas backup format."));
        }
        catch (JsonException)
        {
            return CreateFailure(LocalizedText.Translate("The selected file is not valid JSON."));
        }
    }

    private static bool LooksLikeBackup(JsonElement root)
        => root.ValueKind == JsonValueKind.Object &&
           root.TryGetProperty("library", out var libraryElement) &&
           (libraryElement.ValueKind == JsonValueKind.Object || libraryElement.ValueKind == JsonValueKind.Array);

    private static bool LooksLikeLibrary(JsonElement root)
    {
        if (root.ValueKind == JsonValueKind.Array)
        {
            return true;
        }

        return root.ValueKind == JsonValueKind.Object &&
               (root.TryGetProperty("mediaItems", out _) ||
                root.TryGetProperty("movies", out _) ||
                root.TryGetProperty("series", out _) ||
                root.TryGetProperty("entries", out _));
    }

    private static LibraryBackupModel NormalizeBackup(LibraryBackupModel backup)
    {
        backup.BackupVersion = LibraryBackupModel.CurrentBackupVersion;
        backup.Library ??= LibraryStorageModel.CreateDefault();
        backup.Library.SchemaVersion = LibraryStorageModel.CurrentSchemaVersion;
        backup.Library.MediaItems ??= new();
        backup.Library.Movies ??= new();
        backup.Library.Series ??= new();
        backup.Settings ??= AppSettings.Default;

        return new LibraryBackupModel
        {
            BackupVersion = backup.BackupVersion,
            ExportedAtUtc = backup.ExportedAtUtc == default ? DateTime.UtcNow : backup.ExportedAtUtc,
            Library = LibraryCloneHelper.Clone(backup.Library),
            Settings = LibraryCloneHelper.Clone(backup.Settings)
        };
    }

    private static LibraryImportResult CreateSuccess(LibraryBackupModel backup) => new()
    {
        IsSuccessful = true,
        Backup = LibraryCloneHelper.Clone(backup)
    };

    private static LibraryImportResult CreateFailure(string errorMessage) => new()
    {
        IsSuccessful = false,
        ErrorMessage = errorMessage
    };
}
