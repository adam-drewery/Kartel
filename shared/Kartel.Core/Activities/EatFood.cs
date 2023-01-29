using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Kartel.Attributes;
using Kartel.Entities;
using Kartel.Entities.Items.Containers;
using Kartel.Entities.Items.Foods;

namespace Kartel.Activities;

[Verb("Eat food", "Eating food", "Ate food")]
public class EatFood : Activity
{
    private readonly Func<ICollection<Container>?> _foodContainers;
    
    public EatFood(Person actor, Func<ICollection<Container>?> foodContainers) : base(actor)
    {
        _foodContainers = foodContainers;
    }

    protected override void Update(TimeSpan sinceLastUpdate)
    {
        var intervals = Intervals
            .FromSeconds(300)
            .ForRange(Game.Clock.Time - sinceLastUpdate, Game.Clock.Time);

        foreach (var _ in intervals)
        {
            var containers = _foodContainers() ?? throw new InvalidDataException("No food containers found.");
            var container = containers.First(c => c.Any(x => x is Food));
            var food = container.OfType<Food>().First();
            container.Remove(food);
            Actor.Eat(food);
            
            if (Actor.Needs.Food.Value == byte.MinValue)
                Complete();
        }
    }
}