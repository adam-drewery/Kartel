using Kartel.Environment;
using Kartel.Environment.Topography;
using Kartel.Services;

namespace Kartel.Testing;

public class MockLocaleClient: ILocaleClient
{
    private readonly Game _game;

    public MockLocaleClient(Game game)
    {
        _game = game;
    }

    public Task<Shop> FindStoreAsync((Location Location, StockType BuildingType) @params)
    {
        return Task.FromResult(new Shop(_game));
    }
}