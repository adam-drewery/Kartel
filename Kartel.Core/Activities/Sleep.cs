using System;
using System.Linq;
using Kartel.Attributes;
using Kartel.Entities;

namespace Kartel.Activities;

[Verb("Sleep", "Sleeping", "Slept")]
public class Sleep : Activity
{
    public Sleep(Person actor) : base(actor) { }

    protected override void Update(TimeSpan sinceLastUpdate)
    {
        
        
        var intervals = Intervals
            .FromSeconds(113)
            .ForRange(Game.Clock.Time - sinceLastUpdate, Game.Clock.Time)
            .Count();
        
        var need = Actor.Needs.Sleep;
        
        if (need.Value - intervals > byte.MinValue)
            need.Value -= (byte)intervals;
        else
            need.Value = byte.MinValue;

        if (need.Value == byte.MinValue)
            Complete();
    }
}