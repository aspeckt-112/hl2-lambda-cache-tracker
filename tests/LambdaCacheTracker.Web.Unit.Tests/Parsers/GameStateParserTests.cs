using LambdaCacheTracker.Web.Parsers;
using Microsoft.Extensions.Logging.Abstractions;

namespace LambdaCacheTracker.Web.Unit.Tests.Parsers;

public class GameStateParserTests
{
    private readonly GameStateParser _parser = new(NullLogger<GameStateParser>.Instance);

    [Fact]
    public void GetHexValue_WhenFileContentIsNull_ThrowsArgumentException()
    {
        // Act
        Action act = () => _parser.GetHexValue(null!);

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void GetHexValue_WhenFileContentIsEmpty_ThrowsArgumentException()
    {
        // Act
        Action act = () => _parser.GetHexValue(string.Empty);

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void GetHexValue_WhenFileContentHasNoLambdaData_ReturnsFalseAndNull()
    {
        // Arrange
        const string fileContent = "no lambda data here";

        // Act
        var (parsedSuccessfully, hexValue) = _parser.GetHexValue(fileContent);

        // Assert
        parsedSuccessfully.Should().BeFalse();
        hexValue.Should().BeNull();
    }

    [Fact]
    public void GetHexValue_WhenFileContentHasEmptyLambdaData_ReturnsFalseAndNull()
    {
        // Arrange
        const string fileContent = "\"data\" \"\"";

        // Act
        var (parsedSuccessfully, hexValue) = _parser.GetHexValue(fileContent);

        // Assert
        parsedSuccessfully.Should().BeFalse();
        hexValue.Should().BeNull();
    }

    [Fact]
    public void GetHexValue_WhenFileContentHasInvalidHexValue_ReturnsFalseAndNull()
    {
        // Arrange
        const string fileContent = "\"data\" \"0xnotahexvalue\"";

        // Act
        var (parsedSuccessfully, hexValue) = _parser.GetHexValue(fileContent);

        // Assert
        parsedSuccessfully.Should().BeFalse();
        hexValue.Should().BeNull();
    }

    [Fact]
    public void GetHexValue_WhenFileContentHasValidHexValue_ReturnsTrueAndHexValue()
    {
        // Arrange
        const string fileContent = "\"data\" \"0x1234567890abcdef\"";

        // Act
        var (parsedSuccessfully, hexValue) = _parser.GetHexValue(fileContent);

        // Assert
        parsedSuccessfully.Should().BeTrue();
        hexValue.Should().Be(0x1234567890abcdef);
    }
}