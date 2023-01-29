using Kartel.Configuration;
using Kartel.Logging;
using Kartel.ServiceBase;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Kartel.Locale.Google;

internal static class Program
{
	private static async Task Main(string[] args)
	{
		Log.Logger = new KartelLogConfiguration().CreateLogger();
		
		LocaleDbContext db = new();
		
		if (await db.Database.EnsureCreatedAsync())
			Log.Information("Created database for provider {ProviderName}", db.Database.ProviderName);

		Log.Debug("Applying pending migrations");
		await db.Database.MigrateAsync();

		var networkSettings = Settings.FromArgs<NetworkSettings>(args);
		var googleSettings = Settings.FromArgs<GoogleSettings>(args);
			
		try
		{
			var routePlanner = new StoreFinder(networkSettings.Locale.Server, googleSettings.ApiKey);
			await Endpoint.RunAsync(routePlanner);
		}
		catch (Exception e)
		{
			Log.Fatal(e, "Fatal Exception");
		}
	}
}