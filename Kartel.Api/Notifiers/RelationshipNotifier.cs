using Kartel.Api.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace Kartel.Api.Notifiers;

public class RelationshipNotifier : CollectionNotifier<RelationshipHub>
{
	public RelationshipNotifier(IHubContext<RelationshipHub> hubContext) : base(hubContext) { }

	public override void Watch(Game game) => Watch(game.Characters);
}