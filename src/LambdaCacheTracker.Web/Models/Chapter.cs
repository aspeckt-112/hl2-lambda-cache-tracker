namespace LambdaCacheTracker.Web.Models;

/// <summary>
/// The chapter model.
/// </summary>
/// <param name="Title">The title of the chapter.</param>
/// <param name="VideoLink">The link to the chapter.</param>
/// <param name="Caches">The caches in the chapter.</param>
public record struct Chapter(string Title, string? VideoLink, Cache[] Caches);