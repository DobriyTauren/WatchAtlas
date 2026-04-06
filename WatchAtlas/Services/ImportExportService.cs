using System.Text.Json;
using WatchAtlas.Models.Library;

namespace WatchAtlas.Services;

public class ImportExportService : IImportExportService
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web)
    {
        WriteIndented = true
    };

    public string Export(IEnumerable<LibraryEntry> entries)
        => JsonSerializer.Serialize(entries, JsonOptions);

    public IReadOnlyList<LibraryEntry> Import(string json)
        => JsonSerializer.Deserialize<List<LibraryEntry>>(json, JsonOptions) ?? new List<LibraryEntry>();
}
