using Kartel.Extensions;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.SignalR.Client;
using Serilog;

namespace Kartel.Web.HubClients.Base;

public abstract class HubClient<T> : HubClient
{
	protected HubClient(HubConnectionBuilder builder, IConfiguration configuration) : base(builder, configuration) { }

	public async Task<T> Subscribe(Guid id)
	{
		if (!IsStarted) await Connect();
			
		Log.Information("Subscribing to {EntityName} with ID {ID}", typeof(T).PrettyName(), id);
		var result = await Connection.InvokeAsync<T>(nameof(Subscribe), id);
		Log.Information("Subscription Successful: {@Result}", result);
		return result;
	}
}

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