using Kartel.Configuration;
using Kartel.Environment.Topography;
using Kartel.Services;

namespace Kartel.ServiceBase.Client;

public class ReverseGeocodingClient : ServiceClient, IReverseGeocodingClient
{
    public Task<Location> ReverseGeocode(Location location) => Request<Location>(location);
	
    public ReverseGeocodingClient(IGame game, NetworkSettings settings) : base(game, settings.ReverseGeocoding.Client) { }
}