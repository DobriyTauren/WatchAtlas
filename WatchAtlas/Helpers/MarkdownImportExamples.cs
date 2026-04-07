namespace WatchAtlas.Helpers;

public static class MarkdownImportExamples
{
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
}
