using BingMapsRESTToolkit;
using Kartel.ServiceBase;
using NetMQ.Sockets;
using Serilog;
using Location = Kartel.Environment.Topography.Location;

namespace Kartel.Geocoding.Bing;

public class ReverseGeocoder : Endpoint<Location, Location>
{
	private readonly string _apiKey;

	public ReverseGeocoder(string apiKey, string address)
	{
		_apiKey = apiKey;
		SocketFactory = () => new ResponseSocket(address);
	}
	protected override Func<ResponseSocket> SocketFactory { get; } 

	protected override async Task<Location> Handle(Location location)
	{
		Log.Information("Received request to geocode {Location}", location);

		var routeRequest = new ReverseGeocodeRequest
		{
			BingMapsKey = _apiKey,
			Point = new Coordinate(location.Latitude, location.Longitude)
		};

		var response = await routeRequest.Execute();

		var bingLocation = (response.ResourceSets.FirstOrDefault()
			?.Resources
			?.OfType<BingMapsRESTToolkit.Location>()
			.Where(l => l.ConfidenceLevelType != 0)).MinBy(l => l.ConfidenceLevelType);

		if (bingLocation == null)
			throw new InvalidDataException("No details returned.");

		location.Address.Lines = new[]
		{
			bingLocation.Address.Locality,
			bingLocation.Address.AdminDistrict2,
			bingLocation.Address.CountryRegion
		};

		return location;
	}
}