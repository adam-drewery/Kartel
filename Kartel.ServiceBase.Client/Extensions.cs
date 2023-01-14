using Kartel.Sockets;
using NetMQ;

namespace Kartel.ServiceBase.Client;

internal static class Extensions
{
	public static async Task<TResponse> Request<TResponse>(this NetMQSocket socket, object request)
	{
		var requestBytes = ByteSerializer.Serialize(request);
		socket.SendFrame(requestBytes);
		var responseBytes = await Task.Factory.StartNew(socket.ReceiveFrameBytes);
		return ByteSerializer.Deserialize<TResponse>(responseBytes);
	}
		
	public static async Task<TResponse> Request<TResponse>(this NetMQSocket socket)
	{
		socket.SendFrameEmpty();
		var responseBytes = await Task.Factory.StartNew(socket.ReceiveFrameBytes);
		return ByteSerializer.Deserialize<TResponse>(responseBytes);
	}
}