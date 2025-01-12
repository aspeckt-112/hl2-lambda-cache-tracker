namespace LambdaCacheTracker.Web.Models;

/// <summary>
/// The cache model.
/// </summary>
/// <param name="Number">The cache number.</param>
/// <param name="Obtained">The obtained status.</param>
public record struct Cache(int Number, bool Obtained);