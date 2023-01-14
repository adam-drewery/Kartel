using Kartel.Api.Hubs.Base;
using Kartel.Api.Interfaces;
using Kartel.Entities;

namespace Kartel.Api.Hubs;

public class RelationshipHub : EntityHub<Relationship>, IRelationshipHub
{
	public RelationshipHub(Game game) : base(game) { }

	protected override Task<Relationship> LoadData(Guid id) => Task.FromResult(Game.Characters
		.SelectMany(c => c.Relationships)
		.Single(r => r.Id == id));
}