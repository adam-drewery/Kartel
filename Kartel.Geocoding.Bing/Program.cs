using Kartel.Configuration;
using Kartel.Logging;
using Kartel.ServiceBase;
using Serilog;

namespace Kartel.Geocoding.Bing;

internal static class Program
{
	private static async Task Main(string[] args)
	{
		var bingSettings = Settings.FromArgs<BingSettings>(args);
		var networkSettings = Settings.FromArgs<NetworkSettings>(args);
		
		Log.Logger = new KartelLogConfiguration().CreateLogger();
		await Endpoint.RunAsync(
			new Geocoder(networkSettings.Geocoding.Server, bingSettings.ApiKey), 
			new ReverseGeocoder(networkSettings.ReverseGeocoding.Server, bingSettings.ApiKey));
	}
	
}