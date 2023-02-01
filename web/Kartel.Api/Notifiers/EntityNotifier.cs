using Kartel.Api.Hubs.Base;
using Kartel.Api.Hubs.Base.Interfaces;
using Kartel.EventArgs;
using Microsoft.AspNetCore.SignalR;
using Serilog;

namespace Kartel.Api.Notifiers;

/// <summary>Notifies a client when an entity it is subscribed to changes.</summary>
public abstract class EntityNotifier<THub> : Notifier<THub>, IEntityNotifier where THub : BaseHub
{
	public EntityNotifier(IHubContext<THub> hubContext) : base(hubContext) { }

	protected void OnPropertyChanged(object? sender, PropertyChangedArgs e) => OnPropertyChanged(e);

	public void OnPropertyChanged(PropertyChangedArgs e)
	{
		if (e.SourceId == default) Log.Debug("Notifying property {Property} changed for entity of type {EntityType}", e.PropertyName, e.Source?.GetType().Name ?? "?");
		else Log.Debug("Notifying property {Property} changed for entity of type {EntityType} with ID {ID}", e.PropertyName, e.Source?.GetType().Name ?? "?", e.SourceId);
		
		Clients.Group(e.SourceId.ToString())
			.SendAsync("PropertyChanged", e);
	}
}