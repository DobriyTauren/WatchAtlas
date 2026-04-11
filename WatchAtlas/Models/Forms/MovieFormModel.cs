using System.ComponentModel.DataAnnotations;
using WatchAtlas.Helpers;
using WatchAtlas.Models;
using WatchAtlas.Models.Enums;

namespace WatchAtlas.Models.Forms;

public class MovieFormModel : IValidatableObject
{
    [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = nameof(ValidationMessages.TitleRequired))]
    public string Title { get; set; } = string.Empty;

    public string? CoverImageUrl { get; set; }

    public string? Description { get; set; }

    public string GenresText { get; set; } = string.Empty;

    [Range(1, 10, ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = nameof(ValidationMessages.RatingRange))]
    public int? PersonalRating { get; set; }

    public string? Notes { get; set; }

    [Range(1, int.MaxValue, ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = nameof(ValidationMessages.DurationPositive))]
    public int? DurationMinutes { get; set; }

    public bool IsWatched { get; set; }

    public DateTime? WatchedDate { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (!CoverImageValueHelper.IsValidStoredValue(CoverImageUrl))
        {
            yield return new ValidationResult(
                ValidationMessages.CoverImageInvalid,
                new[] { nameof(CoverImageUrl) });
        }

        if (WatchedDate.HasValue && !IsWatched)
        {
            yield return new ValidationResult(
                ValidationMessages.MovieWatchedDateRequiresStatus,
                new[] { nameof(WatchedDate), nameof(IsWatched) });
        }
    }

    public static MovieFormModel From(MediaItem media, MovieDetails? movieDetails)
    {
        return new MovieFormModel
        {
            Title = media.Title,
            CoverImageUrl = media.CoverImageUrl,
            Description = media.Description,
            GenresText = string.Join(", ", media.Genres),
            PersonalRating = media.PersonalRating,
            Notes = media.Notes,
            DurationMinutes = movieDetails?.DurationMinutes,
            IsWatched = movieDetails?.IsWatched ?? false,
            WatchedDate = movieDetails?.WatchedDate
        };
    }

    public void CopyFrom(MovieFormModel source)
    {
        Title = source.Title;
        CoverImageUrl = source.CoverImageUrl;
        Description = source.Description;
        GenresText = source.GenresText;
        PersonalRating = source.PersonalRating;
        Notes = source.Notes;
        DurationMinutes = source.DurationMinutes;
        IsWatched = source.IsWatched;
        WatchedDate = source.IsWatched ? source.WatchedDate : null;
    }

    public void ApplyTo(MediaItem media, MovieDetails movieDetails)
    {
        media.Type = MediaType.Movie;
        media.Title = Title?.Trim() ?? string.Empty;
        media.CoverImageUrl = CoverImageValueHelper.Normalize(CoverImageUrl);
        media.Description = Normalize(Description);
        media.Genres = GenresText
            .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();
        media.PersonalRating = PersonalRating;
        media.Notes = Normalize(Notes);

        movieDetails.MediaItemId = media.Id;
        movieDetails.DurationMinutes = DurationMinutes;
        movieDetails.IsWatched = IsWatched;
        movieDetails.WatchedDate = IsWatched ? WatchedDate : null;
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
