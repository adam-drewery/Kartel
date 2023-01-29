using Kartel.Configuration;
using Kartel.Environment;
using Kartel.Services;

namespace Kartel.ServiceBase.Client;

public class PropertyMarketClient : ServiceClient, IPropertyMarketClient
{
	public PropertyMarketClient(IGame game, NetworkSettings settings) : base(game, settings.PropertyMarket.Client) { }
	
	public Task<House> NewHouse(int price = 250000)
	{
		return Request<House>();
	}
}