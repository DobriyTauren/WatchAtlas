using WatchAtlas.Models.Library;

namespace WatchAtlas.Services;

public interface IImportExportService
{
    string Export(LibraryBackupModel backup);
    LibraryImportResult Import(string json);
}
