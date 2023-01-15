using System;
using Kartel.Entities;

namespace Kartel.Activities;

public class Drink : Activity
{
    public sealed override DateTime EndTime { get; set; }

    public Drink(Person actor) : base(actor)
    {
        // takes 1 minute to drink stuff
        EndTime = StartTime.AddMinutes(1);
    }

    protected override void Update(TimeSpan sinceLastUpdate) { }
}