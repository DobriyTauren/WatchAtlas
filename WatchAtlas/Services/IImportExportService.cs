using WatchAtlas.Models.Library;

namespace WatchAtlas.Services;

public interface IImportExportService
{
    string Export(IEnumerable<LibraryEntry> entries);
    IReadOnlyList<LibraryEntry> Import(string json);
}
