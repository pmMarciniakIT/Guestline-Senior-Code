using Guestline_hotels_application;
using Guestline_hotels_application.Services.Availability;
using Guestline_hotels_application.Services.Files;
using Guestline_hotels_application.Services.Search;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.AddSingleton<App>();
        services.AddScoped<IAvailabilityService, AvailabilityService>();
        services.AddScoped<IReadFileService, ReadFileService>();
        services.AddScoped<ISearchService, SearchService>();
    })
    .Build();

var app = host.Services.GetRequiredService<App>();
await app.RunAsync(args);