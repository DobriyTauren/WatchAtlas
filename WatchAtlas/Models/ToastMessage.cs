using WatchAtlas.Models.Enums;

namespace WatchAtlas.Models;

public class ToastMessage
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public ToastTone Tone { get; init; } = ToastTone.Info;
    public string Title { get; init; } = string.Empty;
    public string? Description { get; init; }
}
