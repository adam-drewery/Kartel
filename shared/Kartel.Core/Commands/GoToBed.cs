using Kartel.Attributes;
using Kartel.Entities;

namespace Kartel.Commands;

[Verb("Go to bed", "Sleeping", "Went to bed")]
public class GoToBed : Command
{
    public GoToBed(Person actor) : base(actor)
    {
        Activities.Enqueue(new Activities.Move(actor, () => actor.Home));
        Activities.Enqueue(new Activities.Sleep(actor));
    }
}