using LambdaCacheTracker.Web.Parsers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LambdaCacheTracker.Web.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    
    private readonly GameStateParser _gameStateParser;

    public IndexModel(
        ILogger<IndexModel> logger,
        GameStateParser gameStateParser)
    {
        _logger = logger;
        _gameStateParser = gameStateParser;
    }

    [BindProperty] public IFormFile? GamestateFile { get; set; }
    
    public async Task<IActionResult> OnPostAsync()
    {
        if (GamestateFile is null)
        {
            _logger.LogError("No file was uploaded");
            return Page();
        }
        // Don't bother to copy anything, just read the text directly into memory. It's a fucking tiny file.
        using var reader = new StreamReader(GamestateFile.OpenReadStream());
        var fileContent = await reader.ReadToEndAsync();
        
        if (string.IsNullOrWhiteSpace(fileContent))
        {
            _logger.LogError("Uploaded file was empty");
            return Page();
        }
        
        (bool parsedSuccessfully, ulong? hexValue) = _gameStateParser.GetHexValue(fileContent);
        
        if (!parsedSuccessfully)
        {
            _logger.LogError("Failed to parse game state data");
            return Page();
        }
        
        _logger.LogInformation("Parsed game state data successfully: {HexValue}", hexValue);
        
        

        return Page();
    }
}