using System;
using System.Linq;
using Kartel.Attributes;
using Kartel.Entities;
using Kartel.Entities.Items.Containers;
using Kartel.Entities.Items.Foods;
using Kartel.Environment.Topography;
using Kartel.Extensions;

namespace Kartel.Activities;

[Verb("Find something to eat", "Finding something to eat", "Found something to eat")]
public class FindFood : Activity
{
    public Location Location { get; private set; }

    public FindFood(Person actor) : base(actor) { }

    protected override void Update(TimeSpan sinceLastUpdate)
    {
        // how hungry am i?
        var hunger = Actor.Needs.Food.Value;

        var food = Actor.Home.Rooms
            .SelectMany(room => room.Contents)
            .OfType<Fridge>()
            .SelectMany(fridge => fridge)
            .OfType<Food>();

        // i wanna eat 1kg of food
        if (food.Sum(f => f.Weight) < 1.Kilograms())
        {
            
        }

        if (food != null)
        {
            Location = Actor.Home;
            Complete();
        }
    }
}