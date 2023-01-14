using Kartel.Api.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace Kartel.Api.Notifiers;

public class PlayerNotifier : CollectionNotifier<PlayerHub>
{
	public PlayerNotifier(IHubContext<PlayerHub> hubContext) : base(hubContext) { }
		
	public override void Watch(Game game) => Watch(game.Characters);
}