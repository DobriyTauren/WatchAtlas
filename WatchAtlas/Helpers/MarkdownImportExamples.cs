namespace WatchAtlas.Helpers;

public static class MarkdownImportExamples
{
    public static IReadOnlyList<string> MovieImportSteps =>
    [
        LocalizedText.Translate("Enter the exact movie title below and copy the AI prompt."),
        LocalizedText.Translate("Paste the returned markdown here without extra commentary and click Fill Form.")
    ];

    public static IReadOnlyList<string> SeriesImportSteps =>
    [
        LocalizedText.Translate("Enter the exact show title below and copy the AI prompt."),
        LocalizedText.Translate("Paste the returned markdown here without extra commentary and click Fill Form.")
    ];

    public static IReadOnlyList<string> SeasonImportSteps =>
    [
        LocalizedText.Translate("Enter the show title and the seasons you want to add, then copy the AI prompt."),
        LocalizedText.Translate("Paste the returned markdown here as raw markdown with no code fences or extra text.")
    ];

    public static string Movie =>
        """
        # Movie
        Title: The Matrix
        Universe: The Matrix
        Description: A hacker discovers that reality is a simulation.
        Genres: Science Fiction, Action
        Duration: 136
        """;

    public static string MovieAiPrompt =>
        """
        Create import-ready markdown for one movie for my WatchAtlas app.

        Replace [MOVIE TITLE] with the exact movie name before generating.

        Research rules:
        - Use the latest available public information at generation time.
        - If a value cannot be confirmed, omit that line instead of guessing.

        Output rules:
        - Return only raw markdown.
        - Do not use code fences.
        - Do not add explanations or commentary outside the markdown.
        - Do not include poster URLs, image links, or reference/source URLs.
        - Description should be plain text and ideally should not contain links, citations, source mentions, footnotes, or markdown link syntax.
        - Include `Universe:` when the movie clearly belongs to a named shared universe, franchise, or story world. If there is no clear shared universe, omit that line.
        - Keep genres on one comma-separated line.
        - Use duration as minutes only, for example `Duration: 136`.

        Use this structure:

        # Movie
        Title: [MOVIE TITLE]
        Universe:
        Description:
        Genres:
        Duration:

        Create the markdown for: [MOVIE TITLE]
        """;

    public static string Series =>
        """
        # Show
        Title: Arcane
        Universe: League of Legends
        Description: Sisters from Zaun are pulled into the conflict around Hextech.
        Genres: Animation, Fantasy, Drama

        ## Season 1: 2021-11-06
        - Episode 1: Welcome to the Playground
          - Duration: 43
        - Episode 2: Some Mysteries Are Better Left Unsolved
          - Duration: 41

        ## Season 2: 2024-11-09
        - Episode 1: Heavy Is the Crown
          - Duration: 44
        - Episode 2: Watch It All Burn
          - Duration: 42
        """;

    public static string SeriesAiPrompt =>
        """
        Create import-ready markdown for one TV show for my WatchAtlas app.

        Replace [SHOW TITLE] with the exact show name before generating.

        Research rules:
        - Use the latest available public information at generation time.
        - Include every released season and every released episode.
        - Also include officially announced upcoming seasons or episodes when their existence is confirmed by reliable public sources.
        - If upcoming seasons or episodes already have confirmed episode count, titles, release dates, or durations, include those confirmed details too.
        - If only part of the upcoming information is known, include only the confirmed fields and omit the rest.
        - Do not invent unconfirmed seasons, episodes, titles, dates, counts, or durations.

        Output rules:
        - Return only raw markdown.
        - Do not use code fences.
        - Do not add explanations or commentary outside the markdown.
        - Do not include poster URLs, image links, or reference/source URLs.
        - Description should be plain text and ideally should not contain links, citations, source mentions, footnotes, or markdown link syntax.
        - Include `Universe:` when the series clearly belongs to a named shared universe, franchise, or story world. If there is no clear shared universe, omit that line.
        - Keep genres on one comma-separated line.
        - Use episode duration as minutes only, for example `Duration: 42`.
        - Each season heading must be `## Season N: RELEASE DATE`.
        - After the colon, put the season release date or announced release date.
        - Use the most precise confirmed date available after the colon: `YYYY-MM-DD`, otherwise `YYYY-MM`, otherwise `YYYY`.
        - Number episodes starting from 1 inside each season.
        - For unreleased episodes, omit `Duration:` if it is not confirmed yet.
        - If a season is announced but no episode list is confirmed yet, include the season heading without inventing episode entries.

        Use this structure:

        # Show
        Title: [SHOW TITLE]
        Universe:
        Description:
        Genres:

        ## Season 1: 2021-11-06
        - Episode 1: Episode Title
          - Duration: 42
        - Episode 2: Episode Title
          - Duration: 41

        ## Season 2: 2026-10 (announced)
        - Episode 1: Episode Title
          - Duration: 44
        - Episode 2

        Include all released seasons and episodes, plus officially announced upcoming seasons or episodes, for: [SHOW TITLE]
        """;

    public static string Season =>
        """
        ## Season 3: 2026-10
        - Episode 1: First Light
          - Duration: 46
        - Episode 2: The Last Signal
          - Duration: 49
        """;

    public static string SeasonAiPrompt =>
        """
        Create import-ready markdown for additional TV seasons for my WatchAtlas app.

        Replace [SHOW TITLE] and [SEASONS TO ADD] before generating.

        Research rules:
        - Use the latest available public information at generation time.
        - Include every requested season that has already been released.
        - Also include requested seasons or episodes that are officially announced but not yet released when their existence is confirmed by reliable public sources.
        - If upcoming seasons or episodes already have confirmed episode count, titles, release dates, or durations, include those confirmed details too.
        - If only part of the information is known, include only the confirmed fields and omit the rest.
        - Do not invent unconfirmed seasons, episodes, titles, dates, counts, or durations.

        Output rules:
        - Return only raw markdown.
        - Do not use code fences.
        - Do not add commentary before or after the markdown.
        - Start directly with the first season heading.
        - Do not include poster URLs, image links, or reference/source URLs.
        - If any description text is included anywhere, it should be plain text and ideally should not contain links, citations, source mentions, footnotes, or markdown link syntax.
        - Use minutes only for duration, for example `Duration: 47`.
        - Each season heading must be `## Season N: RELEASE DATE`.
        - After the colon, put the season release date or announced release date.
        - Use the most precise confirmed date available after the colon: `YYYY-MM-DD`, otherwise `YYYY-MM`, otherwise `YYYY`.
        - Number episodes starting from 1 inside each season.
        - For unreleased episodes, omit `Duration:` if it is not confirmed yet.
        - If a season is announced but no episode list is confirmed yet, include the season heading without inventing episode entries.

        Use this structure:

        ## Season 3: 2026-10
        - Episode 1: Episode Title
          - Duration: 46

        ## Season 4: 2027 (announced)
        - Episode 1: Episode Title
          - Duration: 49
        - Episode 2

        Create seasons for: [SHOW TITLE]
        Seasons to add: [SEASONS TO ADD]
        """;
}
