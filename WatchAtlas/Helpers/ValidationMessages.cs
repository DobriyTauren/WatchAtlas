namespace WatchAtlas.Helpers;

public static class ValidationMessages
{
    public static string TitleRequired => LocalizedText.Translate("Title is required.");
    public static string RatingRange => LocalizedText.Translate("Rating should be between 1 and 10.");
    public static string DurationPositive => LocalizedText.Translate("Duration must be greater than zero.");
    public static string CoverImageInvalid => LocalizedText.Translate("Enter a valid http/https image URL or upload an image file.");
    public static string MovieWatchedDateRequiresStatus => LocalizedText.Translate("Watched date can only be set when the movie is marked as watched.");
}
