using Kartel.Configuration;
using Kartel.Logging;
using Kartel.ServiceBase;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Kartel.PropertyMarket;

internal static class Program
{
	private static ListingCollector _listingCollector;

	private static async Task Main(string[] args)
	{
		var networkSettings = Settings.FromArgs<NetworkSettings>(args);
		var zooplaSettings = Settings.FromArgs<ZooplaSettings>(args);
		
		Log.Logger = new KartelLogConfiguration().CreateLogger();
		_listingCollector = new ListingCollector(zooplaSettings.ApiKey);

		await using var client = new ZooplaDbContext();

		Log.Debug("Ensuring database created");
		await client.Database.EnsureCreatedAsync();

		Log.Debug("Applying pending migrations");
		await client.Database.MigrateAsync();

		Log.Information("Starting timer with a {Interval}ms interval", _listingCollector.Interval);
		_listingCollector.Collect();

		_listingCollector.Start();
		await Endpoint.RunAsync(new NewHouseFinder(networkSettings.PropertyMarket.Server));
	}
}