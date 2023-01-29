using Kartel.Attributes;
using Kartel.Entities;
using Kartel.Environment.Topography;

namespace Kartel.Commands;

[Verb("Move", "Moving", "Moved")]
public class Move : Command
{
	public Move(Person actor, Location? location) : base(actor)
	{
		Activities.Enqueue(new Activities.MoveToLocation(actor, () => location));
	}
}