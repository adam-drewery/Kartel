using Kartel.Attributes;
using Kartel.Entities;
using Kartel.Environment.Topography;

namespace Kartel.Commands;

[Verb("Scout", "Scouting", "Scouted")]
public class Scout : Command
{
    protected Scout(Person actor, Location location) : base(actor)
    {
        Tasks.Enqueue(new Activities.MoveToLocation(actor, location));
    }
}