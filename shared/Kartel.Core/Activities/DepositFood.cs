using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Kartel.Attributes;
using Kartel.Entities;
using Kartel.Entities.Items.Containers;
using Kartel.Entities.Items.Foods;

namespace Kartel.Activities;

[Verb("Put away food shopping", "Putting away food shopping", "Put away food shopping")]
public class DepositFood : Activity
{
    private IList<Fridge>? _fridgesToRestock;
    
    public DepositFood(Person actor) : base(actor) { }

    protected override void Update(TimeSpan sinceLastUpdate)
    {
        if (Actor.Home == null)
            throw new InvalidDataException("Can't find food for homeless person.");
        
        _fridgesToRestock ??= Actor.Home.Rooms
            .SelectMany(r => r.Contents)
            .OfType<Fridge>()
            .ToList();
        
        var intervals = Intervals
            .FromMilliseconds(20)
            .ForRange(Game.Clock.Time - sinceLastUpdate, Game.Clock.Time);

        foreach (var _ in intervals)
        {
            var food = Actor.Inventory.OfType<Food>().FirstOrDefault();
            
            if (food == null)
            {
                Complete();
                return;
            }
            
            var fridge = _fridgesToRestock.FirstOrDefault(f => f.CanAdd(food));

            if (fridge == null)
            {
                // Well, just drop the food on the floor into oblivion then
                // todo: don't do that
                break;
                
            }
            
            fridge.Add(food);
            Actor.Inventory.Remove(food);
            
            // we've put away all the food
            if (!Actor.Inventory.OfType<Food>().Any())
            {
                Complete();
                break;
            }
        }

        
    }
}