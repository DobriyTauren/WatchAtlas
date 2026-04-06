using System.ComponentModel.DataAnnotations;
using WatchAtlas.Helpers;
using WatchAtlas.Models;
using WatchAtlas.Models.Enums;

namespace WatchAtlas.Models.Forms;

public class MovieFormModel : IValidatableObject
{
    [Required(ErrorMessage = "Title is required.")]
    public string Title { get; set; } = string.Empty;

    public string? CoverImageUrl { get; set; }

    public string? Description { get; set; }

    public string GenresText { get; set; } = string.Empty;

    [Range(1, 10, ErrorMessage = "Rating should be between 1 and 10.")]
    public int? PersonalRating { get; set; }

    public string? Notes { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Duration must be greater than zero.")]
    public int? DurationMinutes { get; set; }

    public bool IsWatched { get; set; }

    public DateTime? WatchedDate { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (!CoverImageValueHelper.IsValidStoredValue(CoverImageUrl))
        {
            yield return new ValidationResult(
                "Enter a valid http/https image URL or upload an image file.",
                new[] { nameof(CoverImageUrl) });
        }

        if (WatchedDate.HasValue && !IsWatched)
        {
            yield return new ValidationResult(
                "Watched date can only be set when the movie is marked as watched.",
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
