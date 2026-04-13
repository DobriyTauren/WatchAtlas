using System.Globalization;
using System.Text.RegularExpressions;
using WatchAtlas.Helpers;
using WatchAtlas.Models.Forms;
using WatchAtlas.Models.Markdown;

namespace WatchAtlas.Services;

public partial class MarkdownImportService : IMarkdownImportService
{
    private static readonly string[] GenericMovieHeadings = ["movie", "film"];
    private static readonly string[] GenericSeriesHeadings = ["series", "show", "tv series", "tv show"];

    public MarkdownImportParseResult<MovieFormModel> ParseMovie(string markdown)
    {
        var lines = PrepareLines(markdown);
        if (lines.Count == 0)
        {
            return MarkdownImportParseResult<MovieFormModel>.Failure(LocalizedText.Translate("Paste some markdown first."));
        }

        var movie = new MovieFormModel();
        var warnings = new List<string>();
        string? currentMultilineField = null;

        foreach (var rawLine in lines)
        {
            if (string.IsNullOrWhiteSpace(rawLine))
            {
                continue;
            }

            var line = NormalizeLine(rawLine);

            if (TryParseHeading(line, out var headingLevel, out var headingText))
            {
                currentMultilineField = null;

                if (headingLevel == 1 && string.IsNullOrWhiteSpace(movie.Title))
                {
                    var extractedTitle = ExtractTitleFromHeading(headingText, GenericMovieHeadings);
                    if (!string.IsNullOrWhiteSpace(extractedTitle))
                    {
                        movie.Title = extractedTitle;
                    }
                }

                continue;
            }

            if (TryParseKeyValue(line, out var key, out var value))
            {
                currentMultilineField = ApplyMovieField(movie, key, value, warnings);
                continue;
            }

            if (!string.IsNullOrWhiteSpace(currentMultilineField))
            {
                AppendMultilineMovieValue(movie, currentMultilineField, line);
            }
        }

        if (string.IsNullOrWhiteSpace(movie.Title))
        {
            return MarkdownImportParseResult<MovieFormModel>.Failure(LocalizedText.Translate("A movie title could not be found. Add a `Title:` line or use the first heading for the title."));
        }

        return MarkdownImportParseResult<MovieFormModel>.Success(movie, warnings);
    }

    public MarkdownImportParseResult<SeriesFormModel> ParseSeries(string markdown)
    {
        var lines = PrepareLines(markdown);
        if (lines.Count == 0)
        {
            return MarkdownImportParseResult<SeriesFormModel>.Failure(LocalizedText.Translate("Paste some markdown first."));
        }

        var series = new SeriesFormModel();
        var warnings = new List<string>();
        SeasonFormModel? currentSeason = null;
        EpisodeFormModel? currentEpisode = null;
        string? currentMultilineField = null;

        foreach (var rawLine in lines)
        {
            if (string.IsNullOrWhiteSpace(rawLine))
            {
                continue;
            }

            var line = NormalizeLine(rawLine);

            if (TryParseHeading(line, out var headingLevel, out var headingText))
            {
                currentMultilineField = null;

                if (TryCreateSeasonFromHeader(headingText, out var headingSeason))
                {
                    currentSeason = headingSeason;
                    currentEpisode = null;
                    series.Seasons.Add(currentSeason);
                    continue;
                }

                if (TryCreateEpisodeFromHeader(headingText, out var headingEpisode))
                {
                    currentSeason ??= CreateFallbackSeason(series);
                    currentEpisode = headingEpisode;
                    currentSeason.Episodes.Add(currentEpisode);
                    continue;
                }

                if (headingLevel == 1 && string.IsNullOrWhiteSpace(series.Title))
                {
                    var extractedTitle = ExtractTitleFromHeading(headingText, GenericSeriesHeadings);
                    if (!string.IsNullOrWhiteSpace(extractedTitle))
                    {
                        series.Title = extractedTitle;
                    }
                }

                continue;
            }

            if (TryCreateSeasonFromBullet(line, out var bulletSeason))
            {
                currentMultilineField = null;
                currentSeason = bulletSeason;
                currentEpisode = null;
                series.Seasons.Add(currentSeason);
                continue;
            }

            if (TryCreateEpisodeFromBullet(line, out var bulletEpisode))
            {
                currentMultilineField = null;
                currentSeason ??= CreateFallbackSeason(series);
                currentEpisode = bulletEpisode;
                currentSeason.Episodes.Add(currentEpisode);
                continue;
            }

            if (TryParseKeyValue(line, out var key, out var value))
            {
                currentMultilineField = ApplySeriesField(series, currentSeason, currentEpisode, key, value, warnings);
                continue;
            }

            if (!string.IsNullOrWhiteSpace(currentMultilineField))
            {
                AppendMultilineSeriesValue(series, currentSeason, currentEpisode, currentMultilineField, line);
            }
        }

        if (string.IsNullOrWhiteSpace(series.Title))
        {
            return MarkdownImportParseResult<SeriesFormModel>.Failure(LocalizedText.Translate("A show title could not be found. Add a `Title:` line or use the first heading for the title."));
        }

        NormalizeSeasonOrdering(series);
        return MarkdownImportParseResult<SeriesFormModel>.Success(series, warnings);
    }

    public MarkdownImportParseResult<IReadOnlyList<SeasonFormModel>> ParseSeasons(string markdown)
    {
        var lines = PrepareLines(markdown);
        if (lines.Count == 0)
        {
            return MarkdownImportParseResult<IReadOnlyList<SeasonFormModel>>.Failure(LocalizedText.Translate("Paste some markdown first."));
        }

        var seasons = new List<SeasonFormModel>();
        var warnings = new List<string>();
        SeasonFormModel? currentSeason = null;
        EpisodeFormModel? currentEpisode = null;
        string? currentMultilineField = null;

        foreach (var rawLine in lines)
        {
            if (string.IsNullOrWhiteSpace(rawLine))
            {
                continue;
            }

            var line = NormalizeLine(rawLine);

            if (TryParseHeading(line, out _, out var headingText))
            {
                currentMultilineField = null;

                if (TryCreateSeasonFromHeader(headingText, out var headingSeason))
                {
                    currentSeason = headingSeason;
                    currentEpisode = null;
                    seasons.Add(currentSeason);
                    continue;
                }

                if (TryCreateEpisodeFromHeader(headingText, out var headingEpisode))
                {
                    currentSeason ??= CreateFallbackSeason(seasons);
                    currentEpisode = headingEpisode;
                    currentSeason.Episodes.Add(currentEpisode);
                    continue;
                }

                continue;
            }

            if (TryCreateSeasonFromBullet(line, out var bulletSeason))
            {
                currentMultilineField = null;
                currentSeason = bulletSeason;
                currentEpisode = null;
                seasons.Add(currentSeason);
                continue;
            }

            if (TryCreateEpisodeFromBullet(line, out var bulletEpisode))
            {
                currentMultilineField = null;
                currentSeason ??= CreateFallbackSeason(seasons);
                currentEpisode = bulletEpisode;
                currentSeason.Episodes.Add(currentEpisode);
                continue;
            }

            if (TryParseKeyValue(line, out var key, out var value))
            {
                if (currentEpisode is not null)
                {
                    currentMultilineField = ApplyEpisodeField(currentEpisode, key, value, warnings, currentSeason?.SeasonNumber);
                }
                else if (currentSeason is not null)
                {
                    currentMultilineField = ApplySeasonField(currentSeason, key, value, warnings);
                }

                continue;
            }

            if (!string.IsNullOrWhiteSpace(currentMultilineField))
            {
                AppendMultilineSeasonValue(currentSeason, currentEpisode, currentMultilineField, line);
            }
        }

        if (seasons.Count == 0)
        {
            return MarkdownImportParseResult<IReadOnlyList<SeasonFormModel>>.Failure(LocalizedText.Translate("No season blocks were found. Add `## Season 1` headings or `- Season 1:` list items."));
        }

        NormalizeSeasonOrdering(seasons);
        return MarkdownImportParseResult<IReadOnlyList<SeasonFormModel>>.Success(seasons, warnings);
    }

    private static List<string> PrepareLines(string markdown)
    {
        if (string.IsNullOrWhiteSpace(markdown))
        {
            return [];
        }

        var normalized = markdown
            .Replace("\r\n", "\n", StringComparison.Ordinal)
            .Replace('\r', '\n');

        return normalized
            .Split('\n')
            .Where(line => !line.TrimStart().StartsWith("```", StringComparison.Ordinal))
            .Select(line => line.TrimEnd())
            .ToList();
    }

    private static string NormalizeLine(string value)
        => value.Trim();

    private static bool TryParseHeading(string line, out int level, out string text)
    {
        level = 0;
        text = string.Empty;

        var match = HeadingRegex().Match(line);
        if (!match.Success)
        {
            return false;
        }

        level = match.Groups["level"].Value.Length;
        text = CleanupInline(match.Groups["text"].Value);
        return true;
    }

    private static bool TryParseKeyValue(string line, out string key, out string value)
    {
        key = string.Empty;
        value = string.Empty;

        var cleaned = TrimListPrefix(line);
        var separatorIndex = cleaned.IndexOf(':');
        if (separatorIndex <= 0)
        {
            return false;
        }

        key = NormalizeKey(cleaned[..separatorIndex]);
        value = CleanupInline(cleaned[(separatorIndex + 1)..]).Trim();
        return !string.IsNullOrWhiteSpace(key);
    }

    private static string? ApplyMovieField(MovieFormModel movie, string key, string value, List<string> warnings)
    {
        switch (key)
        {
            case "title":
                movie.Title = value;
                return null;
            case "cover":
            case "cover image":
            case "cover image url":
            case "image":
            case "poster":
                movie.CoverImageUrl = value;
                return null;
            case "description":
                movie.Description = value;
                return string.IsNullOrWhiteSpace(value) ? nameof(movie.Description) : null;
            case "universe":
            case "franchise":
            case "shared universe":
                movie.Universe = value;
                return string.IsNullOrWhiteSpace(value) ? nameof(movie.Universe) : null;
            case "genres":
                movie.GenresText = NormalizeGenres(value);
                return string.IsNullOrWhiteSpace(value) ? nameof(movie.GenresText) : null;
            case "rating":
                movie.PersonalRating = ParseNullableRating(value, warnings, LocalizedText.Translate("movie"));
                return null;
            case "duration":
            case "runtime":
            case "duration minutes":
                movie.DurationMinutes = ParseNullableDuration(value, warnings, LocalizedText.Translate("movie"));
                return null;
            case "watched":
            case "watched status":
                movie.IsWatched = ParseBoolean(value);
                if (!movie.IsWatched)
                {
                    movie.WatchedDate = null;
                }
                return null;
            case "watched date":
            case "date watched":
                movie.WatchedDate = ParseNullableDate(value, warnings, LocalizedText.Translate("movie"));
                movie.IsWatched = movie.WatchedDate.HasValue || movie.IsWatched;
                return null;
            default:
                return null;
        }
    }

    private static string? ApplySeriesField(
        SeriesFormModel series,
        SeasonFormModel? currentSeason,
        EpisodeFormModel? currentEpisode,
        string key,
        string value,
        List<string> warnings)
    {
        if (currentEpisode is not null)
        {
            return ApplyEpisodeField(currentEpisode, key, value, warnings, currentSeason?.SeasonNumber);
        }

        if (currentSeason is not null && key is "season title" or "season" or "title")
        {
            return ApplySeasonField(currentSeason, key, value, warnings);
        }

        switch (key)
        {
            case "title":
                series.Title = value;
                return null;
            case "cover":
            case "cover image":
            case "cover image url":
            case "image":
            case "poster":
                series.CoverImageUrl = value;
                return null;
            case "description":
                series.Description = value;
                return string.IsNullOrWhiteSpace(value) ? nameof(series.Description) : null;
            case "universe":
            case "franchise":
            case "shared universe":
                series.Universe = value;
                return string.IsNullOrWhiteSpace(value) ? nameof(series.Universe) : null;
            case "genres":
                series.GenresText = NormalizeGenres(value);
                return string.IsNullOrWhiteSpace(value) ? nameof(series.GenresText) : null;
            case "rating":
                series.PersonalRating = ParseNullableRating(value, warnings, LocalizedText.Translate("show"));
                return null;
            default:
                return currentSeason is not null
                    ? ApplySeasonField(currentSeason, key, value, warnings)
                    : null;
        }
    }

    private static string? ApplySeasonField(SeasonFormModel season, string key, string value, List<string> warnings)
    {
        switch (key)
        {
            case "season":
                if (ParsePositiveInteger(value) is { } seasonNumberFromSeasonField)
                {
                    season.SeasonNumber = seasonNumberFromSeasonField;
                    return null;
                }

                season.Title = value;
                return string.IsNullOrWhiteSpace(value) ? nameof(season.Title) : null;
            case "season title":
            case "title":
                season.Title = value;
                return string.IsNullOrWhiteSpace(value) ? nameof(season.Title) : null;
            case "season number":
            case "number":
                if (ParsePositiveInteger(value) is { } seasonNumber)
                {
                    season.SeasonNumber = seasonNumber;
                }
                else
                {
                    warnings.Add(LocalizedText.Format("A season number could not be read from `{0}`.", value));
                }

                return null;
            default:
                return null;
        }
    }

    private static string? ApplyEpisodeField(
        EpisodeFormModel episode,
        string key,
        string value,
        List<string> warnings,
        int? seasonNumber)
    {
        switch (key)
        {
            case "episode":
                if (ParsePositiveInteger(value) is { } episodeNumberFromEpisodeField)
                {
                    episode.EpisodeNumber = episodeNumberFromEpisodeField;
                    return null;
                }

                episode.Title = value;
                return string.IsNullOrWhiteSpace(value) ? nameof(episode.Title) : null;
            case "episode title":
            case "title":
                episode.Title = value;
                return string.IsNullOrWhiteSpace(value) ? nameof(episode.Title) : null;
            case "episode number":
            case "number":
                if (ParsePositiveInteger(value) is { } episodeNumber)
                {
                    episode.EpisodeNumber = episodeNumber;
                }
                else
                {
                    warnings.Add(LocalizedText.Format("An episode number could not be read from `{0}`{1}.", value, FormatSeasonContext(seasonNumber)));
                }

                return null;
            case "duration":
            case "runtime":
                episode.DurationMinutes = ParseNullableDuration(value, warnings, LocalizedText.Format("season {0} episode {1}", seasonNumber ?? 0, episode.EpisodeNumber));
                return null;
            case "watched":
            case "watched status":
                episode.IsWatched = ParseBoolean(value);
                if (!episode.IsWatched)
                {
                    episode.WatchedDate = null;
                }
                return null;
            case "watched date":
            case "date watched":
                episode.WatchedDate = ParseNullableDate(value, warnings, LocalizedText.Format("season {0} episode {1}", seasonNumber ?? 0, episode.EpisodeNumber));
                episode.IsWatched = episode.WatchedDate.HasValue || episode.IsWatched;
                return null;
            default:
                return null;
        }
    }

    private static void AppendMultilineMovieValue(MovieFormModel movie, string fieldName, string line)
    {
        switch (fieldName)
        {
            case nameof(MovieFormModel.Description):
                movie.Description = AppendLine(movie.Description, line);
                break;
            case nameof(MovieFormModel.Universe):
                movie.Universe = AppendLine(movie.Universe, line);
                break;
            case nameof(MovieFormModel.GenresText):
                movie.GenresText = AppendGenreLine(movie.GenresText, line);
                break;
        }
    }

    private static void AppendMultilineSeriesValue(
        SeriesFormModel series,
        SeasonFormModel? currentSeason,
        EpisodeFormModel? currentEpisode,
        string fieldName,
        string line)
    {
        if (currentEpisode is not null)
        {
            AppendMultilineSeasonValue(currentSeason, currentEpisode, fieldName, line);
            return;
        }

        switch (fieldName)
        {
            case nameof(SeriesFormModel.Description):
                series.Description = AppendLine(series.Description, line);
                break;
            case nameof(SeriesFormModel.Universe):
                series.Universe = AppendLine(series.Universe, line);
                break;
            case nameof(SeriesFormModel.GenresText):
                series.GenresText = AppendGenreLine(series.GenresText, line);
                break;
            case nameof(SeasonFormModel.Title) when currentSeason is not null:
                currentSeason.Title = AppendLine(currentSeason.Title, line);
                break;
        }
    }

    private static void AppendMultilineSeasonValue(
        SeasonFormModel? currentSeason,
        EpisodeFormModel? currentEpisode,
        string fieldName,
        string line)
    {
        if (currentEpisode is not null)
        {
            if (fieldName == nameof(EpisodeFormModel.Title))
            {
                currentEpisode.Title = AppendLine(currentEpisode.Title, line);
            }

            return;
        }

        if (currentSeason is not null && fieldName == nameof(SeasonFormModel.Title))
        {
            currentSeason.Title = AppendLine(currentSeason.Title, line);
        }
    }

    private static string AppendLine(string? currentValue, string line)
    {
        var cleaned = CleanupInline(TrimListPrefix(line));
        return string.IsNullOrWhiteSpace(currentValue)
            ? cleaned
            : $"{currentValue}{Environment.NewLine}{cleaned}";
    }

    private static string AppendGenreLine(string? currentValue, string line)
    {
        var cleaned = CleanupInline(TrimListPrefix(line));
        if (string.IsNullOrWhiteSpace(cleaned))
        {
            return currentValue ?? string.Empty;
        }

        return string.IsNullOrWhiteSpace(currentValue)
            ? cleaned
            : $"{currentValue}, {cleaned}";
    }

    private static string NormalizeGenres(string? value)
        => string.Join(", ", SplitGenres(value));

    private static IEnumerable<string> SplitGenres(string? value)
        => (value ?? string.Empty)
            .Split([',', ';', '|', '/'], StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(CleanupInline)
            .Where(item => !string.IsNullOrWhiteSpace(item))
            .Distinct(StringComparer.OrdinalIgnoreCase);

    private static int? ParseNullableRating(string value, List<string> warnings, string context)
    {
        var match = NumberRegex().Match(value);
        if (!match.Success)
        {
            warnings.Add(LocalizedText.Format("A rating could not be read from `{0}` for the {1}.", value, context));
            return null;
        }

        if (int.TryParse(match.Value, out var rating))
        {
            return Math.Clamp(rating, 1, 10);
        }

        warnings.Add(LocalizedText.Format("A rating could not be read from `{0}` for the {1}.", value, context));
        return null;
    }

    private static int? ParseNullableDuration(string value, List<string> warnings, string context)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return null;
        }

        var normalized = value.Trim().ToLowerInvariant();
        var hourMatch = Regex.Match(normalized, @"(?<hours>\d+)\s*h");
        var minuteMatch = Regex.Match(normalized, @"(?<minutes>\d+)\s*m");

        if (hourMatch.Success || minuteMatch.Success)
        {
            var hours = hourMatch.Success ? int.Parse(hourMatch.Groups["hours"].Value, CultureInfo.InvariantCulture) : 0;
            var minutes = minuteMatch.Success ? int.Parse(minuteMatch.Groups["minutes"].Value, CultureInfo.InvariantCulture) : 0;
            return hours * 60 + minutes;
        }

        if (int.TryParse(NumberRegex().Match(normalized).Value, out var rawMinutes))
        {
            return rawMinutes;
        }

        warnings.Add(LocalizedText.Format("A duration could not be read from `{0}` for {1}.", value, context));
        return null;
    }

    private static DateTime? ParseNullableDate(string value, List<string> warnings, string context)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return null;
        }

        if (DateTime.TryParse(value, CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal, out var parsedDate)
            || DateTime.TryParse(value, CultureInfo.CurrentCulture, DateTimeStyles.AssumeLocal, out parsedDate))
        {
            return parsedDate.Date;
        }

        warnings.Add(LocalizedText.Format("A watched date could not be read from `{0}` for {1}.", value, context));
        return null;
    }

    private static bool ParseBoolean(string? value)
    {
        var normalized = value?.Trim().ToLowerInvariant();
        return normalized is "true" or "yes" or "watched" or "completed" or "done" or "1" or "x";
    }

    private static int? ParsePositiveInteger(string? value)
    {
        var match = NumberRegex().Match(value ?? string.Empty);
        return match.Success && int.TryParse(match.Value, out var number) && number > 0
            ? number
            : null;
    }

    private static bool TryCreateSeasonFromHeader(string headingText, out SeasonFormModel season)
        => TryCreateSeasonFromLabel(headingText, out season);

    private static bool TryCreateSeasonFromBullet(string line, out SeasonFormModel season)
        => TryCreateSeasonFromLabel(TrimListPrefix(line), out season);

    private static bool TryCreateEpisodeFromHeader(string headingText, out EpisodeFormModel episode)
        => TryCreateEpisodeFromLabel(headingText, out episode);

    private static bool TryCreateEpisodeFromBullet(string line, out EpisodeFormModel episode)
        => TryCreateEpisodeFromLabel(TrimListPrefix(line), out episode);

    private static bool TryCreateSeasonFromLabel(string value, out SeasonFormModel season)
    {
        season = new SeasonFormModel();

        var match = SeasonRegex().Match(value);
        if (!match.Success)
        {
            return false;
        }

        season.SeasonNumber = int.Parse(match.Groups["number"].Value, CultureInfo.InvariantCulture);
        season.Title = NormalizeNullable(match.Groups["title"].Value);
        return true;
    }

    private static bool TryCreateEpisodeFromLabel(string value, out EpisodeFormModel episode)
    {
        episode = new EpisodeFormModel();

        var match = EpisodeRegex().Match(value);
        if (!match.Success)
        {
            return false;
        }

        episode.EpisodeNumber = int.Parse(match.Groups["number"].Value, CultureInfo.InvariantCulture);
        episode.Title = NormalizeNullable(match.Groups["title"].Value);
        return true;
    }

    private static SeasonFormModel CreateFallbackSeason(SeriesFormModel series)
        => CreateFallbackSeason(series.Seasons);

    private static SeasonFormModel CreateFallbackSeason(List<SeasonFormModel> seasons)
    {
        var season = new SeasonFormModel
        {
            SeasonNumber = seasons.Count == 0 ? 1 : seasons.Max(item => item.SeasonNumber) + 1
        };

        seasons.Add(season);
        return season;
    }

    private static string ExtractTitleFromHeading(string headingText, IEnumerable<string> genericHeadings)
    {
        var normalized = CleanupInline(headingText);
        if (genericHeadings.Contains(normalized.Trim().ToLowerInvariant()))
        {
            return string.Empty;
        }

        var colonIndex = normalized.IndexOf(':');
        if (colonIndex > 0)
        {
            var prefix = normalized[..colonIndex].Trim().ToLowerInvariant();
            if (genericHeadings.Contains(prefix))
            {
                return normalized[(colonIndex + 1)..].Trim();
            }
        }

        return normalized;
    }

    private static string TrimListPrefix(string value)
        => Regex.Replace(value.Trim(), @"^(?:[-*+]|>\s*|- \[[xX ]\]|\d+\.)\s*", string.Empty);

    private static string NormalizeKey(string key)
        => CleanupInline(key).Trim().ToLowerInvariant();

    private static string CleanupInline(string value)
    {
        var cleaned = value
            .Replace("**", string.Empty, StringComparison.Ordinal)
            .Replace("__", string.Empty, StringComparison.Ordinal)
            .Replace("`", string.Empty, StringComparison.Ordinal)
            .Trim();

        cleaned = Regex.Replace(cleaned, @"\[(?<label>[^\]]+)\]\((?<url>[^)]+)\)", "${label}");
        return cleaned.Trim();
    }

    private static string? NormalizeNullable(string? value)
    {
        var normalized = CleanupInline(value ?? string.Empty);
        return string.IsNullOrWhiteSpace(normalized) ? null : normalized;
    }

    private static void NormalizeSeasonOrdering(SeriesFormModel series)
        => NormalizeSeasonOrdering(series.Seasons);

    private static void NormalizeSeasonOrdering(List<SeasonFormModel> seasons)
    {
        var nextFallbackSeasonNumber = seasons.Count == 0 ? 1 : seasons.Max(season => season.SeasonNumber) + 1;

        foreach (var season in seasons.Where(season => season.SeasonNumber <= 0))
        {
            season.SeasonNumber = nextFallbackSeasonNumber++;
        }
    }

    private static string FormatSeasonContext(int? seasonNumber)
        => seasonNumber is > 0 ? LocalizedText.Format(" in season {0}", seasonNumber) : string.Empty;

    [GeneratedRegex(@"^(?<level>#{1,6})\s+(?<text>.+)$", RegexOptions.Compiled)]
    private static partial Regex HeadingRegex();

    [GeneratedRegex(@"\d+", RegexOptions.Compiled)]
    private static partial Regex NumberRegex();

    [GeneratedRegex(@"^(?:season|s)\s*(?<number>\d+)(?:\s*(?:[:\-]|[.])\s*(?<title>.+))?$", RegexOptions.IgnoreCase | RegexOptions.Compiled)]
    private static partial Regex SeasonRegex();

    [GeneratedRegex(@"^(?:episode|ep|e)\s*(?<number>\d+)(?:\s*(?:[:\-]|[.])\s*(?<title>.+))?$", RegexOptions.IgnoreCase | RegexOptions.Compiled)]
    private static partial Regex EpisodeRegex();
}
