using System;
using System.Threading.Tasks;
using Kartel.Commands;
using Kartel.Entities;
using Kartel.Environment.Topography;

namespace Kartel.Activities;

public class MoveToLocation : Activity
{
	public Location Destination { get; }

	public Route Route { get; private set; }

	public Task<Route> DirectionsTask { get; }

	public MoveToLocation(Person actor, Location destination) : base(actor)
	{
		Destination = destination;
		var route = new[] {actor.Location, destination};

		DirectionsTask = Services.Directions.WalkingAsync(route);
	}

	protected override void Update(TimeSpan sinceLastUpdate)
	{
		if (Route == null)
		{
			if (DirectionsTask.IsCompleted)
				Route = DirectionsTask.Result;
			else if (DirectionsTask.IsFaulted)
			{
				Game.OnError("Failed to get directions", DirectionsTask.Exception);
				Complete();
			}
		}
		else
		{
			Actor.Location = Route.LocationAfter(Game.Clock.Time - StartTime);
			if (Actor.Location.Equals(Destination)) Complete();
		}
	}
}