using System;
using Kartel.Attributes;
using Kartel.Entities;
using Kartel.Entities.Items.Foods;
using Kartel.Environment;

namespace Kartel.Activities;

[Verb("Buy food", "Buying food", "Bought food")]
public class BuyFood : Activity
{
    private readonly Func<Shop> _shop;

    public BuyFood(Person actor, Func<Shop> shop) : base(actor)
    {
        _shop = shop;
    }

    protected override void Update(TimeSpan sinceLastUpdate)
    {
        var intervals = Intervals.FromSeconds(20)
            .ForRange(Game.Clock.Time - sinceLastUpdate, Game.Clock.Time);

        // pick up a food item each 20 seconds
        foreach (var _ in intervals)
        {
            Food foodItem = Random.Next(0, 2) == 1
                ? new Sandwich()
                : new Banana();
            
            // todo; pay for the food

            if (Actor.Inventory.CanAdd(foodItem))
                Actor.Inventory.Add(foodItem);
            else 
                break;
        }
    }
}