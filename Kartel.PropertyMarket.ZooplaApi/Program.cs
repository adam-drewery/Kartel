using Kartel.Configuration;
using Kartel.Logging;
using Kartel.ServiceBase;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Kartel.PropertyMarket;

internal static class Program
{
	private static readonly ListingCollector ListingCollector = new();

	private static async Task Main(string[] args)
	{
		var networkSettings = Settings.FromArgs<NetworkSettings>(args);
		
		Log.Logger = new KartelLogConfiguration().CreateLogger();

		await using var client = new ZooplaDbContext();

		Log.Debug("Ensuring database created");
		await client.Database.EnsureCreatedAsync();

		Log.Debug("Applying pending migrations");
		await client.Database.MigrateAsync();

		Log.Information("Starting timer with a {Interval}ms interval", ListingCollector.Interval);
		ListingCollector.Collect();

		ListingCollector.Start();
		await Endpoint.RunAsync(new NewHouseFinder(networkSettings.PropertyMarket.Server));
	}
}