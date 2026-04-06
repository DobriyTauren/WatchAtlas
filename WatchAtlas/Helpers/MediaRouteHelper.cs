using WatchAtlas.Models;
using WatchAtlas.Models.Enums;
using WatchAtlas.Models.Library;

namespace WatchAtlas.Helpers;

public static class MediaRouteHelper
{
    public static string GetCreateHref(MediaType type)
        => type == MediaType.Movie ? "/movies/new" : "/series/new";

    public static string GetDetailsHref(MediaItem media)
        => media.Type == MediaType.Movie ? $"/movies/{media.Id}" : $"/series/{media.Id}";

    public static string GetDetailsHref(LibraryEntry entry)
        => GetDetailsHref(entry.Media);

    public static string GetEditHref(MediaItem media)
        => media.Type == MediaType.Movie ? $"/movies/{media.Id}/edit" : $"/series/{media.Id}/edit";

    public static string GetEditHref(LibraryEntry entry)
        => GetEditHref(entry.Media);
}
