using System;
using System.Threading.Tasks;
using Kartel.Attributes;
using Kartel.Entities;
using Kartel.Environment.Topography;

namespace Kartel.Activities;

[Verb("Move to location", "Moving to location", "Moved to location")]
public class MoveToLocation : Activity
{
    private readonly Person _actor;
    private Task<Route> _directionsTask;

    public Func<Location> Destination { get; }

    public Route Route { get; private set; }

    public MoveToLocation(Person actor, Func<Location> destination) : base(actor)
    {
        _actor = actor;
        Destination = destination;
    }

    protected override void Update(TimeSpan sinceLastUpdate)
    {
        if (_directionsTask == null)
        {
            var routeDistance = Actor.Location.DistanceTo(Destination());
            var duration = TimeSpan.FromSeconds(routeDistance * 1.5); // about 3 m/s

            if (routeDistance < 100)
            {
                _directionsTask = Task.FromResult(
                    Route = new Route
                    {
                        Parts =
                        {
                            new RoutePart(TimeSpan.Zero, _actor.Location),
                            new RoutePart(duration, Destination())
                        }
                    });
            }
            else
            {
                var route = new[] { Actor.Location, Destination() };
                _directionsTask = Services.Directions.WalkingAsync(route);
            }
        }

        if (Route == null)
        {
            if (_directionsTask.IsCompleted)
                Route = _directionsTask.Result;
            else if (_directionsTask.IsFaulted)
            {
                Game.OnError("Failed to get directions", _directionsTask.Exception);
                Complete();
            }
        }
        else
        {
            var newLocation = Route.LocationAfter(Game.Clock.Time - StartTime);

            if (!newLocation.Equals(Actor.Location))
                Actor.Location = newLocation;

            if (Actor.Location.Equals(Destination())) Complete();
        }
    }
}