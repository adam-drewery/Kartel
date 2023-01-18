using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.SignalR.Client;
using Serilog;

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
		
		Connection = builder.WithUrl(url, opt =>
			{
				opt.Transports = HttpTransportType.WebSockets;
			})
			.Build();

		Connection.Reconnected += connectionId =>
		{
			Log.Information("{Type} connection reconnected with ID {ID}", GetType(), connectionId);
			return Task.CompletedTask;
		};
			
		Connection.Reconnecting += exception =>
		{
			Log.Information(exception, "{Type} connection reconnecting", GetType());
			return Task.CompletedTask;
		};
		
		Connection.Closed += exception =>
		{
			Log.Information(exception, "{Type} connection closed", GetType());
			return Task.CompletedTask;
		};
	}
}