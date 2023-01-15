using System;
using Kartel.Entities;

namespace Kartel.Activities;

public class EatFood : Activity
{
    public sealed override DateTime EndTime { get; set; }

    public EatFood(Person actor) : base(actor)
    {
        // takes 20 minutes to eat stuff
        EndTime = StartTime.AddMinutes(20);
    }

    protected override void Update(TimeSpan sinceLastUpdate) { }
}