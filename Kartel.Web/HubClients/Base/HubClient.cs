using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.SignalR.Client;

namespace Kartel.Web.HubClients.Base;

public abstract class HubClient
{
	public bool IsStarted => Connection.State 
		is HubConnectionState.Connected 
		or HubConnectionState.Connecting;
		
	protected HubConnection Connection { get; }

	public Task Disconnect() => Connection.StopAsync();

	public Task Connect()
	{
		return !IsStarted 
			? Connection.StartAsync() 
			: Task.CompletedTask;
	}

	protected HubClient(HubConnectionBuilder builder, IConfiguration configuration)
	{
		var url = $"{configuration["SignalrHub"]}/{GetType().Name.Replace("Client", "Hub").ToLowerInvariant()}";
		Console.WriteLine("ULR IS; " + url);

		Connection = builder.WithUrl(url, opt =>
			{
				opt.Transports = HttpTransportType.WebSockets;
			})
			.Build();

		Connection.Reconnected += connectionId =>
		{
			Console.WriteLine("{0} connection reconnected with ID {1}", GetType(), connectionId);
			return Task.CompletedTask;
		};
			
		Connection.Reconnecting += exception =>
		{
			Console.WriteLine("{0} connection reconnecting", GetType());
			Console.WriteLine(exception);
			return Task.CompletedTask;
		};
		
		Connection.Closed += exception =>
		{
			Console.WriteLine("{0} connection closed", GetType());
			Console.WriteLine(exception);
			return Task.CompletedTask;
		};
	}
}