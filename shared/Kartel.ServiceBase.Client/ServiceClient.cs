using Kartel.MessagePack;
using NetMQ;
using NetMQ.Sockets;

namespace Kartel.ServiceBase.Client;

public class ServiceClient : IDisposable
{
    protected IGame Game { get; }
    protected readonly NetMQSocket Socket;
    protected readonly ByteSerializer ByteSerializer;
	
    public ServiceClient(IGame game, string address)
    {
        Game = game;
        Socket = new RequestSocket(address);
        ByteSerializer = new ByteSerializer(Game);
    }
	
    public async Task<TResponse> Request<TResponse>(object request)
    {
        var requestBytes = ByteSerializer.Serialize(request);
        Socket.SendFrame(requestBytes);
        var responseBytes = await Task.Factory.StartNew(Socket.ReceiveFrameBytes);
        return ByteSerializer.Deserialize<TResponse>(responseBytes);
    }
		
    public async Task<TResponse> Request<TResponse>()
    {
        Socket.SendFrameEmpty();
        var responseBytes = await Task.Factory.StartNew(Socket.ReceiveFrameBytes);
        return ByteSerializer.Deserialize<TResponse>(responseBytes);
    }
	
    public void Dispose() => Socket?.Dispose();
}