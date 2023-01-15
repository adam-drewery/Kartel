using BingMapsRESTToolkit;
using Kartel.Environment.Topography;
using Kartel.ServiceBase;
using NetMQ.Sockets;
using Serilog;
using Location = Kartel.Environment.Topography.Location;
using Route = Kartel.Environment.Topography.Route;

namespace Kartel.Logistics.Bing;

public class RoutePlanner : Endpoint<List<Location>, Route>
{
	private readonly string _apiKey;
	
	protected override Func<ResponseSocket> SocketFactory { get; }
	
	public RoutePlanner(string address, string apiKey)
	{
		_apiKey = apiKey;
		SocketFactory = () => new ResponseSocket(address);
	}

	protected override async Task<Route> Handle(List<Location> requestRoute)
	{
		if (requestRoute.Count == 0)
			throw new InvalidDataException("Empty route request received");

		var start = requestRoute.First();
		var end = requestRoute.Last();

		Log.Information("Received request for route from {Start} to {End}", start, end);

		var routeRequest = new RouteRequest
		{
			BingMapsKey = _apiKey,
			RouteOptions = new RouteOptions {TravelMode = TravelModeType.Driving},
			Waypoints = requestRoute
				.Select(r => new SimpleWaypoint(r.Latitude, r.Longitude))
				.ToList()
		};

		var result = await routeRequest.Execute();
		var route = new Route();

		var itinerary = result.ResourceSets.FirstOrDefault()
			?.Resources
			?.OfType<BingMapsRESTToolkit.Route>()
			.FirstOrDefault()
			?.RouteLegs.FirstOrDefault()
			?.ItineraryItems
			?.ToList();

		if (itinerary == null)
			throw new InvalidDataException("No route found");

		var time = itinerary.First().TimeUtc;
		var locationName = "an unknown location.";
		foreach (var item in itinerary)
		{
			if (item.Details.Any())
			{
				var roadName = item.Details
					?.Where(d => d.Names != null)
					.SelectMany(d => d.Names)
					.FirstOrDefault(n => !string.IsNullOrWhiteSpace(n));

				if (roadName != null)
					locationName = roadName;
			}

			var lat = item.ManeuverPoint.Coordinates[0];
			var lng = item.ManeuverPoint.Coordinates[1];

			var location = new Location(lat, lng)
			{
				Address = new Entities.Address {Lines = new[] {locationName}}
			};

			route.Parts.Add(new RoutePart(item.TimeUtc - time, location));
		}

		Log.Information("Route plotted with {Parts} parts",  route.Parts.Count);
		return route;
	}
}