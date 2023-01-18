using Kartel.Configuration;
using Kartel.Logging;
using Kartel.ServiceBase;
using Serilog;

namespace Kartel.Logistics.Bing;

internal static class Program
{
	private static async Task Main(string[] args)
	{
		Log.Logger = new KartelLogConfiguration().CreateLogger();

		var networkSettings = Settings.FromArgs<NetworkSettings>(args);
		var bingSettings = Settings.FromArgs<BingSettings>(args);
			
		try
		{
			var routePlanner = new RoutePlanner(networkSettings.Logistics.Server, bingSettings.ApiKey);
			await Endpoint.RunAsync(routePlanner);
		}
		catch (Exception e)
		{
			Log.Fatal(e, "Fatal Exception");
		}
	}
}