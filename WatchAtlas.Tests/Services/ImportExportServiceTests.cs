using System.Text.Json;
using WatchAtlas.Models;
using WatchAtlas.Models.Enums;
using WatchAtlas.Models.Library;
using WatchAtlas.Services;
using WatchAtlas.Tests.Support;
using Xunit;

namespace WatchAtlas.Tests.Services;

public class ImportExportServiceTests
{
    private static readonly JsonSerializerOptions WebJson = new(JsonSerializerDefaults.Web);
    private readonly ImportExportService _service = new();

    [Fact]
    public void Import_WithInvalidJson_ReturnsFailure()
    {
        var result = _service.Import("{ invalid");

        Assert.False(result.IsSuccessful);
        Assert.Equal("The selected file is not valid JSON.", result.ErrorMessage);
    }

    [Fact]
    public void Import_WithLibraryPayload_ReturnsBackupAndDefaultSettings()
    {
        var payload = JsonSerializer.Serialize(new LibraryStorageModel
        {
            MediaItems =
            [
                new MediaItem
                {
                    Id = Guid.NewGuid(),
                    Title = "Dexter",
                    Type = MediaType.Series
                }
            ]
        }, WebJson);

        var result = _service.Import(payload);

        Assert.True(result.IsSuccessful);
        Assert.Single(result.Backup.Library.MediaItems);
        Assert.Equal(AppSettings.Default.ThemeMode, result.Backup.Settings.ThemeMode);
        Assert.Equal(LibraryStorageModel.CurrentSchemaVersion, result.Backup.Library.SchemaVersion);
    }

    [Fact]
    public void Import_WithLegacyEntriesArray_MigratesIntoBackupShape()
    {
        var legacyPayload = JsonSerializer.Serialize(new[]
        {
            TestDataFactory.CreateMovieEntry("Legacy Movie", watched: true, durationMinutes: 95)
        }, WebJson);

        var result = _service.Import(legacyPayload);

        Assert.True(result.IsSuccessful);
        Assert.Single(result.Backup.Library.MediaItems);
        Assert.Single(result.Backup.Library.Movies);
        Assert.Empty(result.Backup.Library.Series);
    }

    [Fact]
    public void Export_WritesCompleteBackupShape()
    {
        var mediaId = Guid.NewGuid();
        var backup = new LibraryBackupModel
        {
            Library = new LibraryStorageModel
            {
                MediaItems =
                [
                    new MediaItem
                    {
                        Id = mediaId,
                        Title = "The Bear",
                        Type = MediaType.Movie
                    }
                ],
                Movies =
                [
                    new MovieDetails
                    {
                        MediaItemId = mediaId,
                        DurationMinutes = 30,
                        IsWatched = true
                    }
                ]
            },
            Settings = new AppSettings
            {
                ThemeMode = ThemeMode.Pastel
            }
        };

        var json = _service.Export(backup);
        using var document = JsonDocument.Parse(json);
        var root = document.RootElement;

        Assert.Equal(LibraryBackupModel.CurrentBackupVersion, root.GetProperty("backupVersion").GetInt32());
        Assert.Equal(LibraryStorageModel.CurrentSchemaVersion, root.GetProperty("library").GetProperty("schemaVersion").GetInt32());
        Assert.Equal(1, root.GetProperty("library").GetProperty("mediaItems").GetArrayLength());
        Assert.Equal(1, root.GetProperty("library").GetProperty("movies").GetArrayLength());
        Assert.Equal((int)ThemeMode.Pastel, root.GetProperty("settings").GetProperty("themeMode").GetInt32());
    }
}
