namespace LambdaCacheTracker.Web.Results;

/// <summary>
/// The result of reading the game state from a file.
/// </summary>
public enum ReadGameStateFromFileResult
{
    FailedToReadFileContent,
    EmptyFileContent,
    Success
}