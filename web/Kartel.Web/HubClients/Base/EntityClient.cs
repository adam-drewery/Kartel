using Kartel.EventArgs;
using Microsoft.AspNetCore.SignalR.Client;
using Serilog;

namespace Kartel.Web.HubClients.Base;

public abstract class EntityClient<T> : HubClient<T>
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
	
	public async Task<T> Bind(Guid id, Action callback = null)
	{
		var target = await Subscribe(id);
		PropertyChanged += (_, args) =>
		{
			args.ApplyTo(target);
			callback?.Invoke();
		};
		
		return target;
	}

	public event EventHandler<PropertyChangedArgs> PropertyChanged;
}