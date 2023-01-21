using Kartel.Configuration;
using Kartel.Environment;
using Kartel.Services;
using NetMQ;
using NetMQ.Sockets;

namespace Kartel.ServiceBase.Client;

public class PropertyMarketClient : IPropertyMarketClient, IDisposable
{
	public PropertyMarketClient(NetworkSettings settings)
	{
		_socket = new RequestSocket(settings.PropertyMarket.Client);
	}
	
	public Task<House> NewHouse(int price = 250000)
	{
		return _socket.Request<House>();
	}

	private readonly NetMQSocket _socket;

	public void Dispose() => _socket?.Dispose();
}