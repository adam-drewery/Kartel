using Kartel.Attributes;
using Kartel.Entities;
using Kartel.Environment.Topography;

namespace Kartel.Commands;

[Verb("Train", "Training", "Trained")]
public class Train : Command
{
    public Train(Person actor, Skill skill, Location location) : base(actor)
    {
        Activities.Enqueue(new Activities.MoveToLocation(actor, () => location));
        Activities.Enqueue(new Activities.Train(actor, skill));
    }

    public Train(Person actor, Skill skill) : base(actor)
    {
        Activities.Enqueue(new Activities.Train(actor, skill));
    }
}