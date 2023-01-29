using Kartel.Configuration;
using Kartel.Environment.Topography;
using Kartel.Services;

namespace Kartel.ServiceBase.Client;

public class GeocodingClient : ServiceClient, IGeocodingClient
{
	public Task<Location> Geocode(Location location) => Request<Location>(location);

	public GeocodingClient(IGame game, NetworkSettings settings) : base(game, settings.Geocoding.Client) { }
}