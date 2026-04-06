namespace WatchAtlas.Extensions;

public static class TimeFormattingExtensions
{
    public static string ToReadableDuration(this int minutes)
    {
        if (minutes <= 0)
        {
            return "0 min";
        }

        var hours = minutes / 60;
        var remainingMinutes = minutes % 60;

        return hours switch
        {
            0 => $"{remainingMinutes} min",
            _ when remainingMinutes == 0 => $"{hours}h",
            _ => $"{hours}h {remainingMinutes}m"
        };
    }
}
