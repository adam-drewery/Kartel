using Kartel.Attributes;
using Kartel.Entities;

namespace Kartel.Commands;

[Verb("Socialize", "Socializing", "Socialized")]
public class Socialize : Command
{
    public Socialize(Person actor) : base(actor)
    {
        Activities.Enqueue(new Activities.Move(actor, () => actor.Home));
        Activities.Enqueue(new Activities.TakeDrugs(actor));
    }
}