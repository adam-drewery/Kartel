using Kartel.Attributes;
using Kartel.Entities;
using Kartel.Environment;

namespace Kartel.Commands;

[Verb("Move House", "Moving House", "Moved House")]
public class MoveHouse : Command
{
    private House? House { get; }

    public MoveHouse(Person actor, House house) : base(actor)
    {
        House = house;
            
        // Don't move into other people's houses
        if (!Actor.Estate.Contains(House)) Cancel();
            
        Activities.Enqueue(new Activities.MoveToLocation(actor, () => house));
    }
}