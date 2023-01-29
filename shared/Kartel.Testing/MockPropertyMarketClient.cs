using Audacia.Random.Extensions;
using Kartel.Environment;
using Kartel.Services;

namespace Kartel.Testing;

public class MockPropertyMarketClient : IPropertyMarketClient
{
    private readonly Game _game;
    
    private static readonly Random Random = new();

    public MockPropertyMarketClient(Game game)
    {
        _game = game;
    }

    public Task<House> NewHouse(int price = 250000)
    {
        return Task.FromResult(new House(_game)
        {
            Address =
            {
                Value = string.Join(" ",
                    $"{Random.Next(0, 100)} {Random.Street()}",
                    Random.City(),
                    Random.PostCode())
            }
        });
    }
}