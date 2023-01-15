using System;
using Kartel.Entities;

namespace Kartel.Activities;

public class TakeDrugs : Activity
{
    public sealed override DateTime EndTime { get; set; }

    public TakeDrugs(Person actor) : base(actor)
    {
        EndTime = StartTime.AddMinutes(10);
    }

    protected override void Update(TimeSpan sinceLastUpdate) { }
}