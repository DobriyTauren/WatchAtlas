using WatchAtlas.Models.Forms;
using WatchAtlas.Models.Markdown;

namespace WatchAtlas.Services;

public interface IMarkdownImportService
{
    MarkdownImportParseResult<MovieFormModel> ParseMovie(string markdown);
    MarkdownImportParseResult<SeriesFormModel> ParseSeries(string markdown);
    MarkdownImportParseResult<IReadOnlyList<SeasonFormModel>> ParseSeasons(string markdown);
}
