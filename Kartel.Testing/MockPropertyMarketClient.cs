using Audacia.Random.Extensions;
using Kartel.Environment;
using Kartel.Services;

namespace Kartel.Testing;

public class MockPropertyMarketClient : IPropertyMarketClient
{
    private static readonly Random Random = new();
    
    public Task<Building> NewHouse(int price = 250000)
    {
        return Task.FromResult(new Building
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