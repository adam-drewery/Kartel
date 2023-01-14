using Kartel.EventArgs;
using Kartel.Extensions;
using Microsoft.AspNetCore.SignalR.Client;

namespace Kartel.Web.HubClients.Base;

public abstract class EntityClient<T> : HubClient
{
	protected EntityClient(HubConnectionBuilder builder, IConfiguration config) : base(builder, config)
	{
		Connection.On<PropertyChangedArgs>(nameof(PropertyChanged), args =>
		{
			PropertyChanged?.Invoke(this, args);
			return Task.CompletedTask;
		});
	}

	public event EventHandler<PropertyChangedArgs> PropertyChanged;

	public async Task<T> Subscribe(Guid id)
	{
		if (!IsStarted) await Connect();
			
		Console.WriteLine("Subscribing to {0} with ID {1}", typeof(T).PrettyName(), id);
		var result = await Connection.InvokeAsync<T>(nameof(Subscribe), id);
		Console.WriteLine("Subscription Successful: {0}", result);
		return result;
	}
}