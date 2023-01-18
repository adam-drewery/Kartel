using Kartel.Attributes;
using Kartel.Entities;

namespace Kartel.Commands;

[Verb("Get something to eat", "Getting something to eat", "Got something to eat")]
public class GetFood : Command
{   
    public GetFood(Person actor) : base(actor)
    {
        var findFood = new Activities.FindFood(actor); 
        Activities.Enqueue(findFood);
        
        Activities.Enqueue(new Activities.MoveToLocation(actor, () => findFood.Location));
        Activities.Enqueue(new Activities.EatFood(actor));
    }
}