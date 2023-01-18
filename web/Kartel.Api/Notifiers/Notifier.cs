using Kartel.Api.Hubs.Base;
using Kartel.Api.Hubs.Base.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace Kartel.Api.Notifiers;

public abstract class Notifier<THub> : INotifier where THub : BaseHub
{
	private readonly IHubContext<THub> _hubContext;

	public abstract void Watch(Game game);

	public Notifier(IHubContext<THub> hubContext) => _hubContext = hubContext;

	protected IHubClients Clients => _hubContext.Clients;

	protected IGroupManager Groups => _hubContext.Groups;
}