using Kartel.Environment.Topography;
using Kartel.Services;

namespace Kartel.Testing;

public class MockGeocodingClient : IGeocodingClient
{
    public Task<Location> Geocode(Location location)
    {
        return Task.FromResult(new Location());
    }

    public Task<Location> ReverseGeocode(Location location)
    {
        return Task.FromResult(new Location());
    }
}