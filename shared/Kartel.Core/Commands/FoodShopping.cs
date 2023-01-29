using Kartel.Attributes;
using Kartel.Entities;

namespace Kartel.Commands;

[Verb("Go food shopping", "Going food shopping", "Went food shopping")]
public class FoodShopping : Command
{
    public FoodShopping(Person actor) : base(actor)
    {
        var findShop = new Activities.FindFoodStore(actor);
        Activities.Enqueue(findShop);
        Activities.Enqueue(new Activities.Move(actor, () => findShop.Result));
        
        // todo: wait until the shop is actually open
        Activities.Enqueue(new Activities.BuyFood(actor));
        Activities.Enqueue(new Activities.Move(actor, () => Actor.Home));
        Activities.Enqueue(new Activities.DepositFood(actor));
    }
}