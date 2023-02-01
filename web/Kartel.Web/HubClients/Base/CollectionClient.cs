using Kartel.EventArgs;
using Microsoft.AspNetCore.SignalR.Client;

namespace Kartel.Web.HubClients.Base;

public abstract class CollectionClient<T> : EntityClient<List<T>>
{
	protected CollectionClient(HubConnectionBuilder builder, IConfiguration config) : base(builder, config)
	{
		Connection.On<CollectionChangedArgs>(nameof(CollectionChanged), args =>
		{
			OnCollectionChanged(args);
			return Task.CompletedTask;
		});
	}

	public event EventHandler<CollectionChangedArgs> CollectionChanged;

	protected virtual void OnCollectionChanged(CollectionChangedArgs e)
	{
		CollectionChanged?.Invoke(this, e);
	}
}