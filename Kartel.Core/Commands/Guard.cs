using Kartel.Attributes;
using Kartel.Entities;
using Kartel.Environment.Topography;

namespace Kartel.Commands;

[Verb("Guard", "Guarding", "Guarded")]
public class Guard : Command
{
    public Guard(Person actor, Location location) : base(actor)
    {
        Activities.Enqueue(new Activities.MoveToLocation(actor, () => location));
    }
}