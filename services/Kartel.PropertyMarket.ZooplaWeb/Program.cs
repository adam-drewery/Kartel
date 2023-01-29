using Kartel.Configuration;
using Kartel.Logging;
using Kartel.ServiceBase;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Kartel.PropertyMarket.ZooplaWeb;

internal static class Program
{
    private static readonly ListingCollector ListingCollector = new();

    private static async Task Main(string[] args)
    {
        var networkSettings = Settings.FromArgs<NetworkSettings>(args);
        
        try
        {
            Log.Logger = new KartelLogConfiguration().CreateLogger();

            Log.Debug("Ensuring database created");
            ZooplaDbContext db = new();
            
            if (await db.Database.EnsureCreatedAsync())
                Log.Information("Created database for provider {ProviderName}", db.Database.ProviderName);

            Log.Debug("Applying pending migrations");
            await db.Database.MigrateAsync();

            Log.Information("Starting timer with a {Interval}ms interval", ListingCollector.Interval);

#pragma warning disable CS4014
            ListingCollector.Collect();
#pragma warning restore CS4014

            ListingCollector.Start();
            await Endpoint.RunAsync(new NewHouseFinder(networkSettings.PropertyMarket.Server));
            
            Log.Warning("Exiting");
        }
        catch (Exception e)
        {
            Log.Fatal(e, "Fatal error encountered");
        }
    }
}