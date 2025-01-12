using System.Text.RegularExpressions;
using LambdaCacheTracker.Web.Results;

namespace LambdaCacheTracker.Web.Services;

/// <summary>
/// The GameStateFileService is responsible for reading the game state data from a file.
/// </summary>
public partial class GameStateFileService
{
    private readonly ILogger<GameStateFileService> _logger;

    // ReSharper disable once ConvertToPrimaryConstructor
    /// <summary>
    /// Initializes a new instance of the <see cref="GameStateFileService"/> class.
    /// </summary>
    /// <param name="logger">The logger.</param>
    public GameStateFileService(ILogger<GameStateFileService> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Reads the game state from the given file.
    /// </summary>
    /// <param name="formFile">The game state file.</param>
    /// <returns>The game state value.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the form file is null.</exception>
    public async Task<(ReadGameStateFromFileResult Result, string? Value)> ReadGameStateFromFile(IFormFile? formFile)
    {
        ArgumentNullException.ThrowIfNull(formFile);

        string? fileContent;

        try
        {
            using var reader = new StreamReader(formFile.OpenReadStream());
            fileContent = await reader.ReadToEndAsync();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to read file content");
            return (ReadGameStateFromFileResult.FailedToReadFileContent, null);
        }

        if (string.IsNullOrWhiteSpace(fileContent))
        {
            _logger.LogError("File content was empty");
            return (ReadGameStateFromFileResult.EmptyFileContent, null);
        }

        var dataPattern = DataPatternRegex();

        var match = dataPattern.Match(fileContent);

        if (!match.Success)
        {
            _logger.LogError("Failed to parse game state data");
            return (ReadGameStateFromFileResult.FailedToReadFileContent, null);
        }

        string hexValue = match.Groups[1].Value;

        return (ReadGameStateFromFileResult.Success, hexValue);
    }

    [GeneratedRegex("\"data\"\\s*\"([^\"]+)\"", RegexOptions.Compiled)]
    private static partial Regex DataPatternRegex();
}