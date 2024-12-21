var builder = WebApplication.CreateBuilder(args);

builder
    .Services
    .AddRazorPages();

var app = builder.Build();

app.UseRouting();

app.MapStaticAssets();

app
    .MapRazorPages()
    .WithStaticAssets();

await app.RunAsync();