using Kartel.Attributes;
using Kartel.Entities;

namespace Kartel.Commands;

[Verb("Get something to drink", "Getting something to drink", "Got something to drink")]
public class GetDrink : Command
{
    public GetDrink(Person actor) : base(actor)
    {
        var findFood = new Activities.FindDrink(actor); 
        Activities.Enqueue(findFood);
        
        Activities.Enqueue(new Activities.MoveToLocation(actor, () => findFood.Location));
        Activities.Enqueue(new Activities.Drink(actor));
    }
}