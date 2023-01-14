using System;
using Kartel.Attributes;
using Kartel.Entities;
using Kartel.Environment;

namespace Kartel.Commands;

[Verb("Move House", "Moving House", "Moved House")]
public class MoveHouse : Command
{
    private static readonly TimeSpan TimeToMove = TimeSpan.FromHours(24); // Number of hours it takes to move house
        
    private Building Building { get; }

    public MoveHouse(Person actor, Building building) : base(actor)
    {
        Building = building;
            
        // Don't move into other people's houses
        if (!Actor.Estate.Contains(Building)) Cancel();
            
        Tasks.Enqueue(new Activities.MoveToLocation(actor, building));
    }
}