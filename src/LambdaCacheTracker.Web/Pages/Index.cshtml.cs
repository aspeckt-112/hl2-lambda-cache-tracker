using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LambdaCacheTracker.Web.Pages;

public class IndexModel(ILogger<IndexModel> logger) : PageModel
{
    private readonly ILogger<IndexModel> _logger = logger;
}