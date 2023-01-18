using System;
using System.Linq;
using Kartel.Attributes;
using Kartel.Entities;

namespace Kartel.Activities;

[Verb("Drink", "Drinking", "Drank")]
public class Drink : Activity
{
    public Drink(Person actor) : base(actor) { }
    
    protected override void Update(TimeSpan sinceLastUpdate)
    {
        var intervals = Intervals
            .FromMilliseconds(20)
            .ForRange(Game.Clock.Time - sinceLastUpdate, Game.Clock.Time)
            .Count();

        var need = Actor.Needs.Drink;
        
        if (need.Value - intervals > byte.MinValue)
            need.Value -= (byte)intervals;
        else
            need.Value = byte.MinValue;

        if (need.Value == byte.MinValue)
            Complete();
    }
}