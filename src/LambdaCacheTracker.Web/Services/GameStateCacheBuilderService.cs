using LambdaCacheTracker.Web.Models;
using LambdaCacheTracker.Web.Results;

namespace LambdaCacheTracker.Web.Services;

/// <summary>
/// The GameStateCacheBuilderService is responsible for building the cache data from the game state.
/// </summary>
public class GameStateCacheBuilderService
{
    private readonly ILogger<GameStateCacheBuilderService> _logger;

    // ReSharper disable once ConvertToPrimaryConstructor
    /// <summary>
    /// Initializes a new instance of the <see cref="GameStateCacheBuilderService"/> class.
    /// </summary>
    /// <param name="logger">The logger.</param>
    public GameStateCacheBuilderService(ILogger<GameStateCacheBuilderService> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Builds the obtained caches from the game state.
    /// </summary>
    /// <param name="gameState">The game state.</param>
    /// <returns>An array of chapters with the obtained caches.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the game state is null or empty.</exception>
    public (BuildObtainedCachesResult Result, Chapter[]? Chapters) BuildObtainedCaches(string? gameState)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(gameState);

        ulong? gameStateHex;

        try
        {
            // Hex is zero padded 16 character hexadecimal.
            gameStateHex = Convert.ToUInt64(gameState, 16);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to convert game state to hex value");
            return (BuildObtainedCachesResult.InvalidGameState, null);
        }

        int[] obtainedCaches = GetObtainedCachesFromHexValue(gameStateHex.Value);

        Chapter[] chapters =
        [
            BuildChapter("A Red Letter Day", "https://www.youtube.com/watch?v=8be_82sx5_w&t=0s", obtainedCaches[..1]),
            BuildChapter("Route Kanal", "https://youtu.be/8be_82sx5_w?t=5", obtainedCaches[1..8]),
            BuildChapter("Water Hazard", "https://www.youtube.com/watch?v=8be_82sx5_w&t=76s", obtainedCaches[8..18]),
            BuildChapter("Black Mesa East", "https://www.youtube.com/watch?v=8be_82sx5_w&t=251s", obtainedCaches[18..19]),
            BuildChapter("We Don't Go To Ravenholm", "https://www.youtube.com/watch?v=8be_82sx5_w&t=260s", obtainedCaches[19..23]),
            BuildChapter("Highway 17", "https://www.youtube.com/watch?v=8be_82sx5_w&t=322s", obtainedCaches[23..28]),
            BuildChapter("Sandtraps", "https://www.youtube.com/watch?v=8be_82sx5_w&t=424s", obtainedCaches[28..30]),
            BuildChapter("Nova Prospekt", "https://www.youtube.com/watch?v=8be_82sx5_w&t=465s", obtainedCaches[30..33]),
            BuildChapter("Entanglement", "https://www.youtube.com/watch?v=8be_82sx5_w&t=496s", obtainedCaches[33..34]),
            BuildChapter("Anticitizen One", "https://www.youtube.com/watch?v=8be_82sx5_w&t=505s", obtainedCaches[34..44]),
            BuildChapter("Follow Freeman", "https://www.youtube.com/watch?v=8be_82sx5_w&t=636s", obtainedCaches[44..45])
        ];

        return (BuildObtainedCachesResult.Success, chapters);
    }

    private int[] GetObtainedCachesFromHexValue(ulong gameState)
    {
        // If the game state is 0, return an array of 45 zeros - the user has not obtained any caches.
        if (gameState == 0)
        {
            return new int[45];
        }

        int[] caches = new int[45];

        for (int i = 0; i < 45; i++)
        {
            caches[i] = (gameState & ((ulong)1 << i)) != 0 ? 1 : 0;
        }

        return caches;
    }

    private static Chapter BuildChapter(string title, string? videoLink, int[] caches)
    {
        return new Chapter(title, videoLink, GetObtainedCaches(caches));
    }

    private static Cache[] GetObtainedCaches(int[] caches)
    {
        return caches.Select((cache, index) => new Cache(index + 1, IsObtained(cache))).ToArray();
    }

    private static bool IsObtained(int cache)
    {
        return cache == 1;
    }
}