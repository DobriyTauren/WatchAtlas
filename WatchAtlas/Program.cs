using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using WatchAtlas.Repositories;
using WatchAtlas.Services;
using WatchAtlas.State;
using WatchAtlas;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<IMediaRepository, InMemoryMediaRepository>();
builder.Services.AddScoped<ISettingsRepository, InMemorySettingsRepository>();
builder.Services.AddScoped<IThemeService, ThemeService>();
builder.Services.AddScoped<IStatisticsService, StatisticsService>();
builder.Services.AddScoped<IImageService, ImageService>();
builder.Services.AddScoped<IImportExportService, ImportExportService>();
builder.Services.AddScoped<LibraryState>();
builder.Services.AddScoped<ThemeState>();
builder.Services.AddScoped<FilterState>();
builder.Services.AddScoped<SettingsState>();

await builder.Build().RunAsync();
