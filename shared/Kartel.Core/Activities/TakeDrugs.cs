using System;
using System.Linq;
using Kartel.Attributes;
using Kartel.Entities;

namespace Kartel.Activities;

[Verb("Take some drugs", "Taking drugs", "Took some drugs")]
public class TakeDrugs : Activity
{

    public TakeDrugs(Person actor) : base(actor) { }

    protected override void Update(TimeSpan sinceLastUpdate)
    {
        var intervals = Intervals
            .FromSeconds(1)
            .ForRange(Game.Clock.Time - sinceLastUpdate, Game.Clock.Time)
            .Count();
        
        var need = Actor.Needs.Drugs;
        
        if (need.Value - intervals > byte.MinValue)
            need.Value -= (byte)intervals;
        else
            need.Value = byte.MinValue;

        if (need.Value == byte.MinValue)
            Complete();
    }
}