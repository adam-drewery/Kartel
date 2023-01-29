using Kartel.Configuration;
using Kartel.Environment.Topography;
using Kartel.Services;

namespace Kartel.ServiceBase.Client;

public class LogisticsClient : ServiceClient, ILogisticsClient
{
    public Task<Route> WalkingAsync(params Location[] points)
    {
        return Request<Route>(points);
    }

    public LogisticsClient(IGame game, NetworkSettings settings) : base(game, settings.Logistics.Client) { }
}