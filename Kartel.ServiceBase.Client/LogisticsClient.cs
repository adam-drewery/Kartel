using Kartel.Configuration;
using Kartel.Environment.Topography;
using Kartel.Services;
using NetMQ;
using NetMQ.Sockets;

namespace Kartel.ServiceBase.Client;

public class LogisticsClient : ILogisticsClient, IDisposable
{
	public LogisticsClient(NetworkSettings settings)
	{
		_socket = new RequestSocket(settings.Logistics.Client);
	}

	private readonly NetMQSocket _socket;

	public Task<Route> WalkingAsync(params Location[] points)
	{
		return _socket.Request<Route>(points);
	}

	public void Dispose() => _socket?.Dispose();
}