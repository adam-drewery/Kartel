using Kartel.Configuration;
using Kartel.Environment.Topography;
using Kartel.Extensions;
using Serilog;

namespace Kartel.ServiceBase.Client;

internal static class Program
{
	public static void Main(string[] args)
	{
		var settings = Settings.FromArgs<NetworkSettings>(args);
		
		Console.Clear();
		var propertyMarket = new PropertyMarketClient(settings);
		var logistics = new LogisticsClient(settings);
		var geocoding = new GeocodingClient(settings);

		while(true)
		{
			try
			{
				Console.WriteLine();
					
				Console.WriteLine("Test Service:");
				Console.WriteLine("1. Property Market");
				Console.WriteLine("2. Logistics");
				Console.WriteLine("3. Geocoder");
				Console.WriteLine("4. Reverse Geocoder");

				var key = Console.ReadKey();
				if (key.Key == ConsoleKey.Escape) break;

				Console.Clear();

				if (key.KeyChar == '1')
				{
					var house = propertyMarket.NewHouse();
					Log.Information("Result: {@House}", house);
				}

				if (key.KeyChar == '2')
				{
					var start = DataSet.Cities.Random();
					var end = DataSet.Cities.Except(new[] {start}).Random();
						
					Console.WriteLine("Getting directions from {0} to {1}", start, end);
					Console.WriteLine();
						
					var house = logistics.WalkingAsync(start, end);
					Log.Information("Result: {@House}", house);
				}

				if (key.KeyChar == '3')
				{
					var city = DataSet.Cities.Random();

					var location = new Location(0, 0) {Address = {Value = city.Name}};
					Console.WriteLine("Geocoding {0}", city.Name);
					var house = geocoding.Geocode(location);
					Console.WriteLine("Result: {0}", house);
				}

				if (key.KeyChar == '4')
				{
					var city = DataSet.Cities.Random();
					var location = new Location(city.Latitude, city.Longitude);
					Console.WriteLine("Reverse-geocoding {0}", city.Name);
					var house = geocoding.ReverseGeocode(location);
					Console.WriteLine("Result: {0}", house);
				}
			}
			catch (Exception e)
			{
				Log.Error(e, "Unexpected exception encountered");
			}
		}
	}
}