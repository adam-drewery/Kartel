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

		var settings = Settings.FromArgs<NetworkSettings>(args);
			
		try
		{
			await Endpoint.RunAsync(new RoutePlanner(settings.Logistics.Server));
		}
		catch (Exception e)
		{
			Log.Fatal(e, "Fatal Exception");
		}
	}
}