using Kartel.Api.Hubs.Base;
using Kartel.Entities;

namespace Kartel.Api.Hubs;

public class RelationshipsHub : CollectionHub<Relationship>
{
	public RelationshipsHub(Game game) : base(game) { }

	protected override Task<List<Relationship>> LoadData(Guid id)
	{
		return Task.FromResult(Game.Characters[id].Relationships.ToList());
	}
}