namespace WatchAtlas.Models.Markdown;

public class MarkdownImportApplyResult
{
    public bool IsSuccess { get; init; }
    public string? ErrorMessage { get; init; }

    public static MarkdownImportApplyResult Success()
        => new()
        {
            IsSuccess = true
        };

    public static MarkdownImportApplyResult Failure(string message)
        => new()
        {
            ErrorMessage = message
        };
}
