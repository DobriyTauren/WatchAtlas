using WatchAtlas.Models;

namespace WatchAtlas.Models.Library;

public class LibraryBackupModel
{
    public const int CurrentBackupVersion = 1;

    public int BackupVersion { get; set; } = CurrentBackupVersion;
    public DateTime ExportedAtUtc { get; set; } = DateTime.UtcNow;
    public LibraryStorageModel Library { get; set; } = LibraryStorageModel.CreateDefault();
    public AppSettings Settings { get; set; } = AppSettings.Default;
}
