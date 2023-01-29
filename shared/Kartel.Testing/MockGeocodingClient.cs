using Kartel.Environment.Topography;
using Kartel.Services;

namespace Kartel.Testing;

public class MockGeocodingClient : IGeocodingClient
{
    private readonly Game _game;

    public MockGeocodingClient(Game game)
    {
        _game = game;
    }

    public Task<Location> Geocode(Location location)
    {
        return Task.FromResult(new Location(_game));
    }

    public Task<Location> ReverseGeocode(Location location)
    {
        return Task.FromResult(new Location(_game));
    }
}