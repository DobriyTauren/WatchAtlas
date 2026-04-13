using System.ComponentModel.DataAnnotations;
using WatchAtlas.Helpers;
using WatchAtlas.Models;
using WatchAtlas.Models.Enums;

namespace WatchAtlas.Models.Forms;

public class SeriesFormModel : IValidatableObject
{
    [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = nameof(ValidationMessages.TitleRequired))]
    public string Title { get; set; } = string.Empty;

    public string? CoverImageUrl { get; set; }

    public string? Description { get; set; }

    public string? Universe { get; set; }

    public string GenresText { get; set; } = string.Empty;

    [Range(1, 10, ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = nameof(ValidationMessages.RatingRange))]
    public int? PersonalRating { get; set; }

    public List<SeasonFormModel> Seasons { get; set; } = new();

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (!CoverImageValueHelper.IsValidStoredValue(CoverImageUrl))
        {
            yield return new ValidationResult(
                ValidationMessages.CoverImageInvalid,
                new[] { nameof(CoverImageUrl) });
        }

        var seasonNumbers = new HashSet<int>();

        foreach (var season in Seasons)
        {
            if (season.SeasonNumber <= 0)
            {
                yield return new ValidationResult(
                    LocalizedText.Translate("Season numbers should be greater than zero."),
                    new[] { nameof(Seasons) });
            }

            if (!seasonNumbers.Add(season.SeasonNumber))
            {
                yield return new ValidationResult(
                    LocalizedText.Format("Season {0} appears more than once.", season.SeasonNumber),
                    new[] { nameof(Seasons) });
            }

            var episodeNumbers = new HashSet<int>();
            foreach (var episode in season.Episodes)
            {
                if (episode.EpisodeNumber <= 0)
                {
                    yield return new ValidationResult(
                        LocalizedText.Format("Season {0} contains an episode number that is not greater than zero.", season.SeasonNumber),
                        new[] { nameof(Seasons) });
                }

                if (!episodeNumbers.Add(episode.EpisodeNumber))
                {
                    yield return new ValidationResult(
                        LocalizedText.Format("Season {0} contains duplicate episode numbers.", season.SeasonNumber),
                        new[] { nameof(Seasons) });
                }

                if (episode.DurationMinutes is <= 0)
                {
                    yield return new ValidationResult(
                        LocalizedText.Format("Season {0}, episode {1} must have a positive duration when provided.", season.SeasonNumber, episode.EpisodeNumber),
                        new[] { nameof(Seasons) });
                }

                if (episode.WatchedDate.HasValue && !episode.IsWatched)
                {
                    yield return new ValidationResult(
                        LocalizedText.Format("Season {0}, episode {1} cannot have a watched date unless it is marked as watched.", season.SeasonNumber, episode.EpisodeNumber),
                        new[] { nameof(Seasons) });
                }
            }
        }
    }

    public static SeriesFormModel From(MediaItem media, SeriesDetails? details)
    {
        return new SeriesFormModel
        {
            Title = media.Title,
            CoverImageUrl = media.CoverImageUrl,
            Description = media.Description,
            Universe = details?.Universe,
            GenresText = string.Join(", ", media.Genres),
            PersonalRating = media.PersonalRating,
            Seasons = (details?.Seasons ?? Enumerable.Empty<Season>())
                .OrderBy(season => season.SeasonNumber)
                .Select(SeasonFormModel.From)
                .ToList()
        };
    }

    public void CopyFrom(SeriesFormModel source)
    {
        Title = source.Title;
        CoverImageUrl = source.CoverImageUrl;
        Description = source.Description;
        Universe = source.Universe;
        GenresText = source.GenresText;
        PersonalRating = source.PersonalRating;
        Seasons = source.Seasons
            .OrderBy(season => season.SeasonNumber)
            .Select(season => season.Clone())
            .ToList();
    }

    public void ApplyTo(MediaItem media, SeriesDetails details)
    {
        media.Type = MediaType.Series;
        media.Title = Title?.Trim() ?? string.Empty;
        media.CoverImageUrl = CoverImageValueHelper.Normalize(CoverImageUrl);
        media.Description = Normalize(Description);
        media.Genres = GenresText
            .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();
        media.PersonalRating = PersonalRating;

        details.MediaItemId = media.Id;
        details.Universe = Normalize(Universe);
        details.Seasons = Seasons
            .OrderBy(season => season.SeasonNumber)
            .Select(season => season.ToDomain(media.Id))
            .ToList();
    }

    public void AddSeason()
    {
        Seasons.Add(new SeasonFormModel
        {
            SeasonNumber = Seasons.Count == 0 ? 1 : Seasons.Max(season => season.SeasonNumber) + 1
        });
    }

    public void RemoveSeason(SeasonFormModel season)
        => Seasons.Remove(season);

    public void AddEpisode(SeasonFormModel season)
    {
        season.Episodes.Add(new EpisodeFormModel
        {
            EpisodeNumber = season.Episodes.Count == 0 ? 1 : season.Episodes.Max(episode => episode.EpisodeNumber) + 1
        });
    }

    public void RemoveEpisode(SeasonFormModel season, EpisodeFormModel episode)
        => season.Episodes.Remove(episode);

    public IReadOnlyList<string> AppendImportedSeasons(IEnumerable<SeasonFormModel> seasons)
    {
        var notices = new List<string>();

        foreach (var importedSeason in seasons.OrderBy(season => season.SeasonNumber))
        {
            var clone = importedSeason.Clone();
            var originalSeasonNumber = clone.SeasonNumber;

            if (clone.SeasonNumber <= 0 || Seasons.Any(existing => existing.SeasonNumber == clone.SeasonNumber))
            {
                clone.SeasonNumber = Seasons.Count == 0 ? 1 : Seasons.Max(existing => existing.SeasonNumber) + 1;
                notices.Add(originalSeasonNumber > 0
                    ? LocalizedText.Format("Imported season {0} was added as season {1} because that number is already in use.", originalSeasonNumber, clone.SeasonNumber)
                    : LocalizedText.Format("An imported season was added as season {0}.", clone.SeasonNumber));
            }

            Seasons.Add(clone);
        }

        return notices;
    }

    public void GenerateEpisodes(SeasonFormModel season)
    {
        if (season.GenerateEpisodeCount <= 0)
        {
            return;
        }

        for (var index = 0; index < season.GenerateEpisodeCount; index++)
        {
            AddEpisode(season);
        }
    }

    public void ApplySharedDuration(SeasonFormModel season)
    {
        if (season.SharedDurationMinutes is null or <= 0)
        {
            return;
        }

        foreach (var episode in season.Episodes)
        {
            episode.DurationMinutes = season.SharedDurationMinutes;
        }
    }

    public void MarkSeasonWatched(SeasonFormModel season)
    {
        var watchedDate = DateTime.Today;

        foreach (var episode in season.Episodes)
        {
            episode.IsWatched = true;
            episode.WatchedDate ??= watchedDate;
        }
    }

    public void MarkSeasonUnwatched(SeasonFormModel season)
    {
        foreach (var episode in season.Episodes)
        {
            episode.IsWatched = false;
            episode.WatchedDate = null;
        }
    }

    public void MarkSeriesWatched()
    {
        var watchedDate = DateTime.Today;

        foreach (var episode in Seasons.SelectMany(season => season.Episodes))
        {
            episode.IsWatched = true;
            episode.WatchedDate ??= watchedDate;
        }
    }

    public IReadOnlyList<string> GetValidationMessages()
    {
        var results = new List<ValidationResult>();
        Validator.TryValidateObject(this, new ValidationContext(this), results, validateAllProperties: true);

        return results
            .Select(result => result.ErrorMessage)
            .Where(message => !string.IsNullOrWhiteSpace(message))
            .Distinct()
            .Cast<string>()
            .ToList();
    }

    private static string? Normalize(string? value)
        => string.IsNullOrWhiteSpace(value) ? null : value.Trim();
}

public class SeasonFormModel
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public int SeasonNumber { get; set; }
    public string? Title { get; set; }
    public List<EpisodeFormModel> Episodes { get; set; } = new();
    public int GenerateEpisodeCount { get; set; } = 8;
    public int? SharedDurationMinutes { get; set; }

    public static SeasonFormModel From(Season season)
    {
        return new SeasonFormModel
        {
            Id = season.Id,
            SeasonNumber = season.SeasonNumber,
            Title = season.Title,
            Episodes = season.Episodes
                .OrderBy(episode => episode.EpisodeNumber)
                .Select(EpisodeFormModel.From)
                .ToList()
        };
    }

    public SeasonFormModel Clone()
    {
        return new SeasonFormModel
        {
            Id = Id,
            SeasonNumber = SeasonNumber,
            Title = Title,
            GenerateEpisodeCount = GenerateEpisodeCount,
            SharedDurationMinutes = SharedDurationMinutes,
            Episodes = Episodes
                .OrderBy(episode => episode.EpisodeNumber)
                .Select(episode => episode.Clone())
                .ToList()
        };
    }

    public Season ToDomain(Guid seriesId)
    {
        var seasonId = Id == Guid.Empty ? Guid.NewGuid() : Id;

        return new Season
        {
            Id = seasonId,
            SeriesId = seriesId,
            SeasonNumber = SeasonNumber,
            Title = string.IsNullOrWhiteSpace(Title) ? null : Title.Trim(),
            Episodes = Episodes
                .OrderBy(episode => episode.EpisodeNumber)
                .Select(episode => episode.ToDomain(seasonId))
                .ToList()
        };
    }
}

public class EpisodeFormModel
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public int EpisodeNumber { get; set; }
    public string? Title { get; set; }
    public int? DurationMinutes { get; set; }
    public bool IsWatched { get; set; }
    public DateTime? WatchedDate { get; set; }

    public static EpisodeFormModel From(Episode episode)
    {
        return new EpisodeFormModel
        {
            Id = episode.Id,
            EpisodeNumber = episode.EpisodeNumber,
            Title = episode.Title,
            DurationMinutes = episode.DurationMinutes,
            IsWatched = episode.IsWatched,
            WatchedDate = episode.WatchedDate
        };
    }

    public EpisodeFormModel Clone()
    {
        return new EpisodeFormModel
        {
            Id = Id,
            EpisodeNumber = EpisodeNumber,
            Title = Title,
            DurationMinutes = DurationMinutes,
            IsWatched = IsWatched,
            WatchedDate = WatchedDate
        };
    }

    public Episode ToDomain(Guid seasonId)
    {
        return new Episode
        {
            Id = Id == Guid.Empty ? Guid.NewGuid() : Id,
            SeasonId = seasonId,
            EpisodeNumber = EpisodeNumber,
            Title = string.IsNullOrWhiteSpace(Title) ? null : Title.Trim(),
            DurationMinutes = DurationMinutes,
            IsWatched = IsWatched,
            WatchedDate = IsWatched ? WatchedDate : null
        };
    }
}
