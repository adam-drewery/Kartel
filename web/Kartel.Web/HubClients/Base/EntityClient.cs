using Kartel.EventArgs;
using Kartel.Extensions;
using Microsoft.AspNetCore.SignalR.Client;
using Serilog;

namespace Kartel.Web.HubClients.Base;

public abstract class EntityClient<T> : HubClient
{
	protected EntityClient(HubConnectionBuilder builder, IConfiguration config) : base(builder, config)
	{
		Connection.On<PropertyChangedArgs>(nameof(PropertyChanged), args =>
		{
			Log.Information("Property changed: {PropertyName}", args.PropertyName);
			PropertyChanged?.Invoke(this, args);
			return Task.CompletedTask;
		});
	}

	public event EventHandler<PropertyChangedArgs> PropertyChanged;

	public async Task<T> Subscribe(Guid id)
	{
		if (!IsStarted) await Connect();
			
		Log.Information("Subscribing to {EntityName} with ID {ID}", typeof(T).PrettyName(), id);
		var result = await Connection.InvokeAsync<T>(nameof(Subscribe), id);
		Log.Information("Subscription Successful: {@Result}", result);
		return result;
	}
}