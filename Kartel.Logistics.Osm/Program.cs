using System.Diagnostics.CodeAnalysis;
using Itinero;
using Itinero.IO.Osm;
using Itinero.Osm.Vehicles;
using Kartel.Configuration;
using Kartel.Environment.Topography;
using Kartel.Logging;
using Kartel.Sockets;
using NetMQ;
using NetMQ.Sockets;
using Serilog;
using Route = Kartel.Environment.Topography.Route;
using Timer = System.Timers.Timer;

namespace Kartel.Logistics.Osm;

/*
 * This service isn't finished yet, it doesn't work at all but even if it did, nodes don't have location names yet.
 */

class Program
{
	public static string OsmDataPath = @"../../../great-britain-latest.osm.pbf";
		
	[SuppressMessage("ReSharper", "FunctionNeverReturns")]
	private static void Main(string[] args)
	{
		var settings = Settings.FromArgs<NetworkSettings>(args);
		Log.Logger = new KartelLogConfiguration().CreateLogger();
			
		var stream = new FileInfo(OsmDataPath).OpenRead();
		var length = stream.Length;

		var timer = new Timer {Interval = 1000};
		timer.Elapsed += (_, _) =>
		{  
			var fraction = (double)stream.Position / length;
			var percent = fraction * 100d;
				
			if (!(Math.Abs(percent - 100d) < 0.01))
				Log.Information("Read stream {Progress}% ({Size}MB)", percent.ToString("F2"), stream.Position / 1024 / 1024);
		};
		timer.Start();
			
		var routerDb = new RouterDb();
			
		Log.Information("Loading {Size}MB of map data from {Path}", length / 1024 / 1024, OsmDataPath);
		routerDb.LoadOsmData(stream, false, false, Vehicle.Car);

		timer.Stop();
		Log.Information("OSM data loaded");

		try
		{
			routerDb.AddContracted(Vehicle.Car.Fastest());
		}
		catch (Exception e)
		{
			Log.Error(e, "Error encountered calculating route");
		}

		using var responseSocket = new ResponseSocket(settings.Logistics.Server);
		while (true)
		{
			var requestBytes = responseSocket.ReceiveFrameBytes();
			var requestedRoute = ByteSerializer.Deserialize<List<Location>>(requestBytes);
				
			if (requestedRoute.Count == 0)
			{
				Log.Error("Empty route request received");
				continue;
			}

			var start = requestedRoute.First();
			var end = requestedRoute.Last();
			Log.Information("Received request for route from {Start} to {End}", start, end);
				
			var router = new Router(routerDb);
			var profile = Vehicle.Car.Fastest(); // the default OSM car profile.
			var startPoint = router.Resolve(profile, (float) start.Latitude, (float) start.Longitude, 100F);
			var endPoint = router.Resolve(profile, (float) end.Latitude, (float) end.Longitude, 100F);

			var result = router.Calculate(profile, startPoint, endPoint);
			var shape = result.Shape.Zip(result.ShapeMeta).Select(x => new {Shape = x.First, Meta = x.Second}).ToList();

			Log.Information("Calculated route with {Length} nodes", result.ShapeMeta.Length);

			var route = new Route();

			foreach (var node in shape)
			{
				// foreach (var attribute in node.Meta.Attributes) 
				// 	Log.Warning(attribute.Key + ":" + attribute.Value);

				var location = new Location(node.Shape.Latitude, node.Shape.Longitude);
				route.Parts.Add(new RoutePart(TimeSpan.FromSeconds(node.Meta.Time), location));
			}

			var bytes = ByteSerializer.Serialize(route);


			Log.Information("Sending directions from {Start} to {End} ({Steps} steps)", start, end, route.Parts.Count);
			responseSocket.SendFrame(bytes);
		}
	}
}