using System.Globalization;
using System.Text.RegularExpressions;

namespace LambdaCacheTracker.Web.Parsers;

public partial class GameStateParser
{
    private readonly ILogger<GameStateParser> _logger;

    public GameStateParser(ILogger<GameStateParser> logger)
    {
        _logger = logger;
    }

    public (bool ParsedSuccessfully, ulong? HexValue) GetHexValue(string fileContent)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(fileContent);

        Regex dataPattern = DataPatternRegex();

        var match = dataPattern.Match(fileContent);

        if (!match.Success)
        {
            _logger.LogError("Failed to parse game state data");
            return (false, null);
        }

        string hexValue = match.Groups[1].Value;

        if (string.IsNullOrWhiteSpace(hexValue))
        {
            _logger.LogError("Game state data was empty");
            return (false, null);
        }

        if (hexValue.StartsWith("0x")
            && ulong.TryParse(hexValue.AsSpan(2), NumberStyles.HexNumber, null, out ulong result))
        {
            return (true, result);
        }

        _logger.LogError("Failed to parse game state data");

        return (false, null);
    }

    [GeneratedRegex("\"data\"\\s*\"([^\"]+)\"", RegexOptions.Compiled)]
    private static partial Regex DataPatternRegex();
}