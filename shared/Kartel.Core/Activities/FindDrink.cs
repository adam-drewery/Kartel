using System;
using Kartel.Attributes;
using Kartel.Entities;
using Kartel.Environment.Topography;

namespace Kartel.Activities;

[Verb("Find something to drink", "Finding something to drink", "Found something to drink")]
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