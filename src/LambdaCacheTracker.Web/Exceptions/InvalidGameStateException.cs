namespace LambdaCacheTracker.Web.Exceptions;

/// <summary>
/// The exception that is thrown when the game state is invalid.
/// </summary>
public class InvalidGameStateException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="InvalidGameStateException"/> class.
    /// </summary>
    /// <param name="message"></param>
    public InvalidGameStateException(string message) : base(message)
    {
    }
}