namespace WatchAtlas.Models.Library;

public class LibraryImportResult
{
    public bool IsSuccessful { get; init; }
    public string? ErrorMessage { get; init; }
    public LibraryBackupModel Backup { get; init; } = new();
}
