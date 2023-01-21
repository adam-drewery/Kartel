using System;
using System.Linq;
using Kartel.Attributes;
using Kartel.Entities;

namespace Kartel.Activities;

[Verb("Eat food", "Eating food", "Ate food")]
public class EatFood : Activity
{
    public EatFood(Person actor) : base(actor) { }

    protected override void Update(TimeSpan sinceLastUpdate)
    {
        var intervals = Intervals
            .FromMilliseconds(5)
            .ForRange(Game.Clock.Time - sinceLastUpdate, Game.Clock.Time)
            .Count();

        var need = Actor.Needs.Food;
        
        if (need.Value - intervals > byte.MinValue)
            need.Value -= (byte)intervals;
        else
            need.Value = byte.MinValue;

        if (need.Value == byte.MinValue)
            Complete();
    }
}