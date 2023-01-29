using Kartel.Configuration;
using Kartel.Environment;
using Kartel.Environment.Topography;
using Kartel.Services;

namespace Kartel.ServiceBase.Client;

public class LocaleClient : ServiceClient, ILocaleClient
{
    public Task<Shop> FindStoreAsync((Location Location, StockType BuildingType) @params)
    {
        return Request<Shop>(@params);
    }

    public LocaleClient(IGame game, NetworkSettings settings) : base(game, settings.Locale.Client) { }
}