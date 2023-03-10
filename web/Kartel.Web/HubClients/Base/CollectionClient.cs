using Kartel.EventArgs;
using Kartel.Observables;
using Microsoft.AspNetCore.SignalR.Client;
using Serilog;

namespace Kartel.Web.HubClients.Base;

public abstract class CollectionClient<T> : HubClient<List<T>> where T : GameObject
{
	protected CollectionClient(HubConnectionBuilder builder, IConfiguration config) : base(builder, config)
	{
		Connection.On<CollectionChangedArgs>(nameof(CollectionChanged), args =>
		{
			Log.Information("Collection changed: {ChangeType}", args.CollectionChangeType);
			CollectionChanged?.Invoke(this, args);
			return Task.CompletedTask;
		});
	}
	
	public async Task Bind(Guid id, ObservableQueue<T> queue, Action callback = null)
	{
		var items = await Subscribe(id);
		queue.Enqueue(items);
		CollectionChanged += (_, args) =>
		{
			args.ApplyTo(queue);
			callback?.Invoke();
		};
	}

	public event EventHandler<CollectionChangedArgs> CollectionChanged;
}