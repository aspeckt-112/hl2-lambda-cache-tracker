using LambdaCacheTracker.Web.Results;
using LambdaCacheTracker.Web.Services;
using Microsoft.Extensions.Logging.Abstractions;

namespace LambdaCacheTracker.Web.Unit.Tests.Services;

public class GameStateCacheBuilderServiceTests
{
    private readonly GameStateCacheBuilderService _gameStateCacheBuilderService =
        new(new NullLogger<GameStateCacheBuilderService>());

    [Fact]
    public void BuildCacheList_WhenGameStateIsNull_ThrowsArgumentException()
    {
        // Arrange
        string? gameState = null;

        // Act
        var act = () => _gameStateCacheBuilderService.BuildObtainedCaches(gameState);

        // Assert
        act.Should().Throw<ArgumentException>();
    }
    
    [Fact]
    public void BuildCacheList_WhenGameStateIsEmpty_ThrowsArgumentException()
    {
        // Arrange
        string gameState = string.Empty;

        // Act
        var act = () => _gameStateCacheBuilderService.BuildObtainedCaches(gameState);

        // Assert
        act.Should().Throw<ArgumentException>();
    }
    
    [Fact]
    public void BuildCacheList_WhenGameStateIsInvalid_ReturnsInvalidGameState()
    {
        // Arrange
        string gameState = "invalid";

        // Act
        var (result, _) = _gameStateCacheBuilderService.BuildObtainedCaches(gameState);

        // Assert
        result.Should().Be(BuildObtainedCachesResult.InvalidGameState);
    }
    
    [Fact]
    public void BuildCacheList_WhenGameStateIsZero_AllCachesObtainedAreFalse()
    {
        // Arrange
        string gameState = "0x0000000000000000";
        
        // Act
        var (_, chapters) = _gameStateCacheBuilderService.BuildObtainedCaches(gameState);

        // Assert
        chapters.Should().HaveCount(12);
        chapters.Should().OnlyContain(chapter => chapter.Caches.All(cache => !cache.Obtained));
    }
    
    [Fact]
    public void BuildCacheList_WhenGameStateIsAllCachesObtained_AllCachesObtainedAreTrue()
    {
        // Arrange
        string gameState = "0x00001FFFFFFFFFFF";
        
        // Act
        var (_, chapters) = _gameStateCacheBuilderService.BuildObtainedCaches(gameState);

        // Assert
        chapters.Should().HaveCount(12);
        chapters.Should().OnlyContain(chapter => chapter.Caches.All(cache => cache.Obtained));
    }
    
    [Fact]
    public void BuildCacheList_WhenFirstCacheIsObtained_FirstCacheIsObtained()
    {
        // Arrange
        string gameState = "0x0000000000000001";
        
        // Act
        var (_, chapters) = _gameStateCacheBuilderService.BuildObtainedCaches(gameState);

        // Assert
        chapters.Should().HaveCount(12);
        var caches = chapters.SelectMany(chapter => chapter.Caches).ToList();
        caches.First().Obtained.Should().BeTrue();
        // The rest of the caches should be false
        caches.Skip(1).Should().OnlyContain(cache => !cache.Obtained);
    }
    
    [Fact]
    public void BuildCacheList_WhenSecondCacheIsObtained_SecondCacheIsObtained()
    {
        // Arrange
        string gameState = "0x0000000000000002";
        
        // Act
        var (_, chapters) = _gameStateCacheBuilderService.BuildObtainedCaches(gameState);

        // Assert
        chapters.Should().HaveCount(12);
        var caches = chapters.SelectMany(chapter => chapter.Caches).ToList();
        caches.Skip(1).First().Obtained.Should().BeTrue();
        // The rest of the caches should be false
        caches.Take(1).Should().OnlyContain(cache => !cache.Obtained);
        caches.Skip(2).Should().OnlyContain(cache => !cache.Obtained);
    }
    
    [Fact]
    public void BuildCacheList_WhenCacheTwentyToThirtyObtained_TwentyToThirtyAreObtained()
    {
        // Arrange
        string gameState = "0x000000003FF00000";
        
        // Act
        var (_, chapters) = _gameStateCacheBuilderService.BuildObtainedCaches(gameState);

        // Assert
        chapters.Should().HaveCount(12);
        var caches = chapters.SelectMany(chapter => chapter.Caches).ToList();
        caches.Skip(20).Take(10).Should().OnlyContain(cache => cache.Obtained);
        // The rest of the caches should be false
        caches.Take(20).Should().OnlyContain(cache => !cache.Obtained);
        caches.Skip(30).Should().OnlyContain(cache => !cache.Obtained);
    }
    
    [Fact]
    public void BuildCacheList_WhenFirstFiveCachesObtained_FirstFiveCachesAreObtained()
    {
        // Arrange
        string gameState = "0x000000000000001F";
        
        // Act
        var (_, chapters) = _gameStateCacheBuilderService.BuildObtainedCaches(gameState);

        // Assert
        chapters.Should().HaveCount(12);
        var caches = chapters.SelectMany(chapter => chapter.Caches).ToList();
        caches.Take(5).Should().OnlyContain(cache => cache.Obtained);
        // The rest of the caches should be false
        caches.Skip(5).Should().OnlyContain(cache => !cache.Obtained);
    }
    
    [Fact]
    public void BuildCacheList_WhenCachesFiveToTenAreObtained_But_OneToFiveAreNot_OneToFiveAreNotObtained()
    {
        // Arrange
        string gameState = "0x00000000000003E0";
        
        // Act
        var (_, chapters) = _gameStateCacheBuilderService.BuildObtainedCaches(gameState);

        // Assert
        chapters.Should().HaveCount(12);
        var caches = chapters.SelectMany(chapter => chapter.Caches).ToList();
        caches.Take(5).Should().OnlyContain(cache => !cache.Obtained);
        caches.Skip(5).Take(5).Should().OnlyContain(cache => cache.Obtained);
    }
}