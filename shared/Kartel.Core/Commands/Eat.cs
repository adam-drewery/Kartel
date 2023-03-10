using System.IO;
using System.Linq;
using Kartel.Activities;
using Kartel.Attributes;
using Kartel.Entities;
using Kartel.Entities.Items.Containers;
using Kartel.Entities.Items.Foods;

namespace Kartel.Commands;

[Verb("Get something to eat", "Getting something to eat", "Got something to eat")]
public class Eat : Command
{   
    public Eat(Person actor) : base(actor)
    {
        if (Actor.Home == null)
            throw new InvalidDataException("Can't find food for homeless person.");
        
        var foodAtHome = Actor.Home.Rooms
            .SelectMany(r => r.Contents.OfType<Fridge>())
            .SelectMany(f => f.OfType<Food>());
        
        if (!foodAtHome.Any())
        {
            Log.Information("{ActorName} ({ActorID}) has no food at home. Going food shopping", Actor.Name, actor.Id);
            
            var findFoodStore = new FindFoodStore(actor);
            Activities.Enqueue(findFoodStore);
            Activities.Enqueue(new MoveToLocation(actor, () => findFoodStore.Result));
            Activities.Enqueue(new BuyFood(actor));
            Activities.Enqueue(new MoveToLocation(actor, () => actor.Home));
            Activities.Enqueue(new DepositFood(actor));
        }
        
        var findFood = new FindFood(actor); 
        Activities.Enqueue(findFood);
        
        Activities.Enqueue(new MoveToLocation(actor, () => findFood.Location));
        Activities.Enqueue(new EatFood(actor, () => findFood.FoodContainers));
    }
}