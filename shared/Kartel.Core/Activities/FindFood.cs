using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Kartel.Attributes;
using Kartel.Entities;
using Kartel.Entities.Items.Containers;
using Kartel.Entities.Items.Foods;
using Kartel.Environment.Topography;

namespace Kartel.Activities;

[Verb("Find something to eat", "Finding something to eat", "Found something to eat")]
public class FindFood : Activity
{
    public Location Location { get; private set; }
    
    public ICollection<Container> FoodContainers { get; private set; }

    public FindFood(Person actor) : base(actor) { }

    protected override void Update(TimeSpan sinceLastUpdate)
    {
        var locations = Actor.Home.Rooms
            .SelectMany(room => room.Contents)
            .OfType<Fridge>()
            .Where(f => f.Container.Any(i => i is Food))
            .ToList<Container>();

        if (locations.Any())
        {
            Location = Actor.Home;
            FoodContainers = locations;
            Complete();
        }
        else
        {
            throw new InvalidDataException("Failed to find any food at home.");
        }
    }
}