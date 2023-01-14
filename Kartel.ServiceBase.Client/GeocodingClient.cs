using Kartel.Configuration;
using Kartel.Environment.Topography;
using Kartel.Services;
using NetMQ;
using NetMQ.Sockets;

namespace Kartel.ServiceBase.Client;

public class GeocodingClient : IGeocodingClient, IDisposable
{
	public GeocodingClient(NetworkSettings settings)
	{
		_geocodeSocket = new RequestSocket(settings.Geocoding.Client);
		_reverseGeocodeSocket = new RequestSocket(settings.ReverseGeocoding.Client);
	}
	
	private readonly NetMQSocket _geocodeSocket;
	private readonly NetMQSocket _reverseGeocodeSocket;

	public Task<Location> Geocode(Location location) => _geocodeSocket.Request<Location>(location);

	public Task<Location> ReverseGeocode(Location location) => _reverseGeocodeSocket.Request<Location>(location);

	public void Dispose()
	{
		_geocodeSocket?.Dispose();
		_reverseGeocodeSocket?.Dispose();
	}
}