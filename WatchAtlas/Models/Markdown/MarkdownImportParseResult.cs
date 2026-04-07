namespace WatchAtlas.Models.Markdown;

public class MarkdownImportParseResult<T>
{
    public T? Value { get; init; }
    public string? ErrorMessage { get; init; }
    public IReadOnlyList<string> Warnings { get; init; } = Array.Empty<string>();
    public bool IsSuccessful => string.IsNullOrWhiteSpace(ErrorMessage) && Value is not null;

    public static MarkdownImportParseResult<T> Success(T value, IReadOnlyList<string>? warnings = null)
        => new()
        {
            Value = value,
            Warnings = warnings ?? Array.Empty<string>()
        };

    public static MarkdownImportParseResult<T> Failure(string message)
        => new()
        {
            ErrorMessage = message
        };
}
