using LambdaCacheTracker.Web.Results;
using LambdaCacheTracker.Web.Services;
using LambdaCacheTracker.Web.Unit.Tests.Data.FormFile;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging.Abstractions;

namespace LambdaCacheTracker.Web.Unit.Tests.Services;

public class GameStateFileServiceTests
{
    private readonly GameStateFileService _gameStateFileService = new(new NullLogger<GameStateFileService>());

    [Fact]
    public async Task ReadGameStateFromFile_WhenFileIsNull_ThrowsArgumentNullException()
    {
        // Arrange
        IFormFile? formFile = null;

        // Act
        var act = async () => await _gameStateFileService.ReadGameStateFromFile(formFile);

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task ReadGameStateFromFile_WhenUnableToOpenReadStream_ReturnsFailedToReadFileContent()
    {
        // Act
        (var result, string? _) = await _gameStateFileService.ReadGameStateFromFile(new ExceptionFormFile());

        // Assert
        result.Should().Be(ReadGameStateFromFileResult.FailedToReadFileContent);
    }

    [Fact]
    public async Task ReadGameStateFromFile_WhenFileContentIsEmpty_ReturnsEmptyFileContent()
    {
        // Act
        (var result, string? _) = await _gameStateFileService.ReadGameStateFromFile(new EmptyFormFile());

        // Assert
        result.Should().Be(ReadGameStateFromFileResult.EmptyFileContent);
    }

    [Fact]
    public async Task ReadGameStateFromFile_WhenDataIsInvalid_ReturnsFailedToReadFileContent()
    {
        // Act
        (var result, string? _) = await _gameStateFileService.ReadGameStateFromFile(new BadDataFormFile());

        // Assert
        result.Should().Be(ReadGameStateFromFileResult.FailedToReadFileContent);
    }

    [Fact]
    public async Task ReadGameStateFromFile_WhenDataIsValid_ReturnsGameState()
    {
        // Arrange
        string expectedGameState = "0x00000008C2C5D3C1";

        // Act
        (var result, string? value) = await _gameStateFileService.ReadGameStateFromFile(new GoodFormFileData());

        // Assert
        result.Should().Be(ReadGameStateFromFileResult.Success);
        value.Should().Be(expectedGameState);
    }
}