using System;
using Kartel.Entities;
using Kartel.Environment.Topography;

namespace Kartel.Activities;

public class FindDrink : Activity
{
    public Location Location { get; private set; }

    public FindDrink(Person actor) : base(actor)
    {
    }

    protected override void Update(TimeSpan sinceLastUpdate)
    {
        // where are the places I might find some food?
        // todo: expand on this
        Location = Actor.Home;
        Complete();
    }
}