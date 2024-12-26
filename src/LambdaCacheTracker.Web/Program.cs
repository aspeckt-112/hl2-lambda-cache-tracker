using LambdaCacheTracker.Web.Builders;
using LambdaCacheTracker.Web.Parsers;

var builder = WebApplication.CreateBuilder(args);

builder
    .Services
    .AddRazorPages();

builder
    .Services
    .AddScoped<GameStateParser>()
    .AddScoped<ULongToBinaryParser>()
    .AddScoped<ChapterBuilder>();

WebApplication app = builder.Build();

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