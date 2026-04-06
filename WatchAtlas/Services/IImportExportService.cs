using WatchAtlas.Models.Library;

namespace WatchAtlas.Services;

public interface IImportExportService
{
    string Export(LibraryStorageModel storage);
    LibraryStorageModel Import(string json);
}
