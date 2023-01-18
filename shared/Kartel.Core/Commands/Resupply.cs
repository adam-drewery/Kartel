using Kartel.Attributes;
using Kartel.Entities;
using Kartel.Environment;

namespace Kartel.Commands;

[Verb("Resupply", "Resupplying", "Resupplied")]
public class Resupply : Command
{
    public Resupply(Person actor, Person supplier, Building stockpile) : base(actor)
    {
        Activities.Enqueue(new Activities.MoveToLocation(actor, () => stockpile));
    }
}