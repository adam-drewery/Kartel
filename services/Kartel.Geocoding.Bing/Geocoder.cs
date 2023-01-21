using BingMapsRESTToolkit;
using Kartel.ServiceBase;
using NetMQ.Sockets;
using Serilog;
using Location = Kartel.Environment.Topography.Location;

namespace Kartel.Geocoding.Bing;

public class Geocoder : Endpoint<Location, Location>
{
	private readonly string _apikey;

	public Geocoder(string address, string apikey)
	{
		_apikey = apikey;
		SocketFactory = () => new ResponseSocket(address);
	}

	protected override Func<ResponseSocket> SocketFactory { get; }

	protected override async Task<Location> Handle(Location location)
	{
		Log.Information("Received request to geocode {Result}", location);

		var geocodeRequest = new GeocodeRequest
		{
			BingMapsKey = _apikey,
			Query = location.Address.ToString()
		};

		var response = await geocodeRequest.Execute();

		var bingLocation = (response.ResourceSets.FirstOrDefault()
				?.Resources
				?.OfType<BingMapsRESTToolkit.Location>()
				.Where(l => l.ConfidenceLevelType != 0) ?? 
					Array.Empty<BingMapsRESTToolkit.Location>())
			.MinBy(l => l.ConfidenceLevelType);

		if (bingLocation == null)
			throw new InvalidDataException("No details returned.");

		location.Latitude = bingLocation.Point.Coordinates[0];
		location.Longitude = bingLocation.Point.Coordinates[1];
		location.Address.Lines = new[]
		{
			bingLocation.Address.Locality,
			bingLocation.Address.AdminDistrict2,
			bingLocation.Address.CountryRegion
		};

		return location;
	}
}