using Kartel.Api.Hubs.Base;
using Kartel.Api.Hubs.Base.Interfaces;
using Kartel.EventArgs;
using Microsoft.AspNetCore.SignalR;

namespace Kartel.Api.Notifiers;

/// <summary>Notifies a client when an entity it is subscribed to changes.</summary>
public abstract class EntityNotifier<THub> : Notifier<THub>, IEntityNotifier where THub : BaseHub
{
	public EntityNotifier(IHubContext<THub> hubContext) : base(hubContext) { }

	protected void OnPropertyChanged(object sender, PropertyChangedArgs e) => OnPropertyChanged(e);

	public void OnPropertyChanged(PropertyChangedArgs e)
	{
		Clients.Group(e.SourceId.ToString())
			.SendAsync(nameof(OnPropertyChanged), e.SourceId, e.PropertyName, e.NewValue);
	}
}