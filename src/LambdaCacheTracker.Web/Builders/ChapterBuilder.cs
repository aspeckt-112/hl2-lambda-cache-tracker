using LambdaCacheTracker.Web.Models;

namespace LambdaCacheTracker.Web.Builders;

public class ChapterBuilder
{
    private readonly ILogger<ChapterBuilder> _logger;

    public ChapterBuilder(ILogger<ChapterBuilder> logger)
    {
        _logger = logger;
    }

    public Chapter[] Build(int[]? caches)
    {
        if (caches is null)
        {
            throw new NotImplementedException();
        }

        Chapter[] chapters =
        [
            BuildPointInsertion(),
            BuildARedLetterDay(caches),
            BuildRouteKanal(caches[1..8])
        ];

        return chapters;
    }

    private static Chapter BuildPointInsertion()
    {
        return new Chapter("Point Insertion", []);
    }

    private Chapter BuildARedLetterDay(int[] caches)
    {
        int redLetterDayCache = caches[0];

        return new Chapter(
            "A Red Letter Day",
            [
                new LambdaCache(IsObtained(redLetterDayCache))
            ]);
    }

    private Chapter BuildRouteKanal(int[] caches)
    {
        return new Chapter(
            "Route Kanal",
            caches.Select(cache => new LambdaCache(IsObtained(cache))).ToArray());
    }

    private bool IsObtained(int cache)
    {
        return cache == 1;
    }
}