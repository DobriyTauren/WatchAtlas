namespace WatchAtlas.Helpers;

public static class MarkdownImportExamples
{
    public static IReadOnlyList<string> MovieImportSteps =>
    [
        "Copy the AI prompt below and paste it into ChatGPT, Claude, or another assistant.",
        "Replace the placeholder with the exact movie title you want to import.",
        "Paste the returned markdown here without extra commentary and click Fill Form."
    ];

    public static IReadOnlyList<string> SeriesImportSteps =>
    [
        "Copy the AI prompt below and paste it into ChatGPT, Claude, or another assistant.",
        "Replace the placeholder with the exact series title you want to import.",
        "Paste the returned markdown here without extra commentary and click Fill Form."
    ];

    public static IReadOnlyList<string> SeasonImportSteps =>
    [
        "Copy the season prompt below and ask your assistant for the missing seasons only.",
        "Keep the answer as raw markdown with no code fences or extra text.",
        "Paste the result here to append the new seasons to the current series draft."
    ];

    public static string Movie =>
        """
        # Movie
        Title: The Matrix
        Cover: https://example.com/matrix.jpg
        Description: A hacker discovers that reality is a simulation.
        Genres: Science Fiction, Action
        Rating: 9/10
        Duration: 136 min
        Watched: yes
        Watched Date: 2026-04-01
        Notes: Rewatch before starting the sequel.
        """;

    public static string MovieAiPrompt =>
        """
        Create import-ready markdown for one movie for my WatchAtlas app.

        Replace [MOVIE TITLE] with the exact movie name before generating.

        Output rules:
        - Return only raw markdown.
        - Do not wrap the answer in code fences.
        - Do not add explanations, notes, or commentary outside the markdown.
        - If a value is unknown, omit that line instead of guessing.
        - Keep genres on one comma-separated line.
        - Use dates only in YYYY-MM-DD format.
        - Use duration as minutes only, for example `Duration: 136`.
        - Use `Watched: yes` or `Watched: no` only if that status is clearly known.
        - Include `Watched Date:` only when an exact watch date is known.

        Use exactly this structure:

        # Movie
        Title: [MOVIE TITLE]
        Cover:
        Description:
        Genres:
        Rating:
        Duration:
        Watched:
        Watched Date:
        Notes:

        Create the markdown for: [MOVIE TITLE]
        """;

    public static string Series =>
        """
        # Series
        Title: Arcane
        Cover: https://example.com/arcane.jpg
        Description: Sisters from Zaun are pulled into the conflict around Hextech.
        Genres: Animation, Fantasy, Drama
        Rating: 10
        Notes: Keep special episodes marked as watched.

        ## Season 1: Book One
        - Episode 1: Welcome to the Playground
          - Duration: 43
          - Watched: yes
          - Watched Date: 2026-03-01
        - Episode 2: Some Mysteries Are Better Left Unsolved
          - Duration: 41
          - Watched: yes

        ## Season 2
        - Episode 1: Heavy Is the Crown
          - Duration: 44
        - Episode 2: Watch It All Burn
          - Duration: 42
        """;

    public static string SeriesAiPrompt =>
        """
        Create import-ready markdown for one TV series for my WatchAtlas app.

        Replace [SERIES TITLE] with the exact show name before generating.

        Output rules:
        - Return only raw markdown.
        - Do not wrap the answer in code fences.
        - Do not add explanations, notes, or commentary outside the markdown.
        - If a value is unknown, omit that line instead of guessing.
        - Keep genres on one comma-separated line.
        - Use dates only in YYYY-MM-DD format.
        - Use episode duration as minutes only, for example `Duration: 42`.
        - Use `Watched: yes` or `Watched: no` only if that status is clearly known.
        - Include `Watched Date:` only when an exact watch date is known.

        Use exactly this structure:

        # Series
        Title: [SERIES TITLE]
        Cover:
        Description:
        Genres:
        Rating:
        Notes:

        ## Season 1: Optional Season Title
        - Episode 1: Episode Title
          - Duration: 42
          - Watched: no

        ## Season 2
        - Episode 1: Episode Title
          - Duration: 44

        Include all released seasons and episodes for: [SERIES TITLE]
        """;

    public static string Season =>
        """
        ## Season 3: Final Chapter
        - Episode 1: First Light
          - Duration: 46
        - Episode 2: The Last Signal
          - Duration: 49
          - Watched: yes
          - Watched Date: 2026-04-05
        """;

    public static string SeasonAiPrompt =>
        """
        Create import-ready markdown for additional TV seasons for my WatchAtlas app.

        Replace [SERIES TITLE] and [SEASONS TO ADD] before generating.

        Output rules:
        - Return only raw markdown.
        - Do not use code fences.
        - Do not add commentary before or after the markdown.
        - Start directly with the first season heading.
        - If a value is unknown, omit that line instead of guessing.
        - Use minutes only for duration, for example `Duration: 47`.
        - Use `Watched: yes` or `Watched: no` only if that status is clearly known.
        - Include `Watched Date:` only when an exact watch date is known.

        Use exactly this structure:

        ## Season 3: Optional Season Title
        - Episode 1: Episode Title
          - Duration: 46

        ## Season 4
        - Episode 1: Episode Title
          - Duration: 49

        Create seasons for: [SERIES TITLE]
        Seasons to add: [SEASONS TO ADD]
        """;
}
