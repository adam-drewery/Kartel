using Kartel.Environment.Topography;
using Kartel.Services;

namespace Kartel.Testing;

public class MockLogisticsClient : ILogisticsClient
{
    public Task<Route> WalkingAsync(Location[] points)
    {
        return Task.FromResult(new Route
        {
            Parts =
            {
                new RoutePart(TimeSpan.Zero, points.First()),
                new RoutePart(TimeSpan.FromSeconds(1), points.Last())
            }
        });
    }
}