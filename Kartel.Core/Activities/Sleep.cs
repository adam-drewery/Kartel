using System;
using Kartel.Entities;

namespace Kartel.Activities;

public class Sleep : Activity
{
    public sealed override DateTime EndTime { get; set; }

    public Sleep(Person actor) : base(actor)
    {
        EndTime = StartTime.AddHours(9);
    }

    protected override void Update(TimeSpan sinceLastUpdate) { }
}