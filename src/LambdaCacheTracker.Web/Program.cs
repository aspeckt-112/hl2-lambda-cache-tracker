using LambdaCacheTracker.Web.Services;

var builder = WebApplication.CreateBuilder(args);

builder
    .Services
    .AddRazorPages();

var services = builder.Services;

services
    .AddScoped<GameStateFileService>()
    .AddScoped<GameStateCacheBuilderService>();

var app = builder.Build();

app.UseRouting();

app.MapStaticAssets();

app.UseStaticFiles(new StaticFileOptions()
{
    OnPrepareResponse = ctx =>
    {
        ctx.Context.Response.Headers.Append("Cache-Control", "public,max-age=604800"); // Cache for 7 days
    }
});

app
    .MapRazorPages()
    .WithStaticAssets();

await app.RunAsync();