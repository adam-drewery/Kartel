using NetMQ;
using NetMQ.Sockets;

namespace Kartel.Sockets;

public class Address : Address<ResponseSocket, RequestSocket>
{
	internal Address() { }
}

public class Address<TServer, TClient>
	where TServer : NetMQSocket
	where TClient : NetMQSocket
{
	internal Address() { }

	public Func<TServer> Server { get; internal set; }

	public Func<TClient> Client { get; internal set; }
}