using LambdaCacheTracker.Web.Models;
using LambdaCacheTracker.Web.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using LambdaCacheTracker.Web.Services;

namespace LambdaCacheTracker.Web.Pages;

/// <summary>
/// The index page model.
/// </summary>
[ValidateAntiForgeryToken]
public class IndexModel : PageModel
{
    private readonly GameStateFileService _gameStateFileService;
    private readonly GameStateCacheBuilderService _gameStateCacheBuilderService;
    
    [BindProperty]
    public IFormFile? Upload { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="IndexModel"/> class.
    /// </summary>
    /// <param name="gameStateFileService">The game state file service.</param>
    /// <param name="gameStateCacheBuilderService">The game state cache builder service.</param>
    public IndexModel(
        GameStateFileService gameStateFileService,
        GameStateCacheBuilderService gameStateCacheBuilderService)
    {
        _gameStateFileService = gameStateFileService;
        _gameStateCacheBuilderService = gameStateCacheBuilderService;
    }
    
    public async Task<IActionResult> OnPostAsync()
    {
        if (Upload is null)
        {
            return new JsonResult(new { success = false, message = "No file was uploaded" });
        }
        
        (ReadGameStateFromFileResult result, string? value) = await _gameStateFileService.ReadGameStateFromFile(Upload);

        return result switch
        {
            ReadGameStateFromFileResult.FailedToReadFileContent => new JsonResult(new
            {
                success = false, message = "Failed to read the file content"
            }),
            ReadGameStateFromFileResult.EmptyFileContent => new JsonResult(new
            {
                success = false, message = "The file content is empty"
            }),
            ReadGameStateFromFileResult.Success => new JsonResult(new
            {
                success = true, message = "File uploaded successfully", gameState = value
            }),
            _ => throw new ArgumentOutOfRangeException()
        };
    }
    
    public Task<IActionResult> OnGetObtainedCachesAsync([FromQuery] string gameState)
    {
        if (string.IsNullOrWhiteSpace(gameState))
        {
            return Task.FromResult<IActionResult>(new JsonResult(new
            {
                success = false, message = "GameState is empty"
            }));
        }

        (BuildObtainedCachesResult result, Chapter[]? chapters) = _gameStateCacheBuilderService.BuildObtainedCaches(gameState);
        
        return result switch
        {
            BuildObtainedCachesResult.InvalidGameState => Task.FromResult<IActionResult>(new BadRequestResult()),
            BuildObtainedCachesResult.Success => Task.FromResult<IActionResult>(Partial("_ChaptersPartial", chapters)),
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}