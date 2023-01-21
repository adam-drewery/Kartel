using Kartel.Attributes;
using Kartel.Entities;

namespace Kartel.Commands;

[Verb("Take Drugs", "Taking drugs", "Took drugs")]
public class TakeDrugs : Command
{
    public TakeDrugs(Person actor) : base(actor)
    {
        Activities.Enqueue(new Activities.Move(actor, () => actor.Home));
        Activities.Enqueue(new Activities.TakeDrugs(actor));
    }
}