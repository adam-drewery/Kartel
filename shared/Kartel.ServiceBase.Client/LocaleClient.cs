using Kartel.Configuration;
using Kartel.Environment;
using Kartel.Environment.Topography;
using Kartel.Services;
using NetMQ;
using NetMQ.Sockets;

namespace Kartel.ServiceBase.Client;

public class LocaleClient : ILocaleClient, IDisposable
{
    public LocaleClient(NetworkSettings settings)
    {
        _socket = new RequestSocket(settings.Locale.Client);
    }

    private readonly NetMQSocket _socket;

    public Task<Shop> FindStoreAsync((Location Location, StockType BuildingType) @params)
    {
        return _socket.Request<Shop>(@params);
    }

    public void Dispose() => _socket?.Dispose();
}