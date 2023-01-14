using System.Diagnostics.CodeAnalysis;
using Kartel.Serialization;
using NetMQ;
using NetMQ.Sockets;
using Serilog;

namespace Kartel.ServiceBase;

public static class Endpoint
{
	public static async Task RunAsync<T>() where T : IEndpoint, new() => await new T().RunAsync();

	public static async Task RunAsync(params IEndpoint[] endpoints)
	{
		await Task.WhenAll(endpoints.Select(e => e.RunAsync()));
	}
}

public abstract class Endpoint<TRequest, TResponse> : IEndpoint
{
	protected abstract Task<TResponse> Handle(TRequest request);

	protected abstract Func<ResponseSocket> SocketFactory { get; }

	public async Task RunAsync()
	{
		using var socket = SocketFactory();
		Log.Information("Listening on {Endpoint}", socket.Options.LastEndpoint);
		await RunAsync(socket);
	}

	[SuppressMessage("ReSharper", "FunctionNeverReturns")]
	private async Task RunAsync(NetMQSocket socket)
	{
		while (true)
		{
			try
			{
				var requestBytes = socket.ReceiveFrameBytes();
				var request = ByteSerializer.Deserialize<TRequest>(requestBytes);
				var response = await Handle(request);

				if (response == null)
				{
					socket.SendFrameEmpty();
					continue;
				}

				var responseBytes = ByteSerializer.Serialize(response);
				socket.SendFrame(responseBytes);
			}
			catch (Exception e)
			{
				Log.Error(e, "Error encountered receiving frame");
				socket.SendFrameEmpty();
			}
		}
	}
}

public abstract class Endpoint<TResponse> : IEndpoint
{
	private readonly Lazy<ResponseSocket> _socket;

	protected abstract Task<TResponse> OnRequest();

	protected ResponseSocket Socket => _socket.Value;

	protected abstract Func<ResponseSocket> SocketFactory { get; }

	protected Endpoint()
	{
		_socket = new Lazy<ResponseSocket>(() => SocketFactory());
	}

	public async Task RunAsync()
	{
		using var socket = SocketFactory();
		Log.Information("Listening on {Endpoint}", socket.Options.LastEndpoint);
		await RunAsync(socket);
	}

	protected virtual Task OnWaiting() => Task.CompletedTask;

	[SuppressMessage("ReSharper", "FunctionNeverReturns")]
	private async Task RunAsync(INetMQSocket socket)
	{

		while (true)
		{
			try
			{
				await OnWaiting();
				socket.ReceiveFrameBytes();
				var response = await OnRequest();

				if (response == null)
				{
					socket.SendFrameEmpty();
					continue;
				}

				var responseBytes = ByteSerializer.Serialize(response);
				socket.SendFrame(responseBytes);
			}
			catch (Exception e)
			{
				Log.Error(e, "Error encountered receiving frame");
				socket.SendFrameEmpty();
			}
		}
	}
}