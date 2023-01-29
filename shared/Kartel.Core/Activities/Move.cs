using System;
using System.Threading.Tasks;
using Kartel.Attributes;
using Kartel.Entities;
using Kartel.Environment.Topography;

namespace Kartel.Activities;

[Verb("Move to location", "Moving to location", "Moved to location")]
public class Move : Activity
{
    private readonly Person _actor;
    private Task<Route> _directionsTask;

    public Func<Location> Destination { get; }

    public Route Route { get; private set; }

    public Move(Person actor, Func<Location> destination) : base(actor)
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
                Log.Error(_directionsTask.Exception, "Failed to get directions");
                Complete();
            }
        }
        else
        {
            var newLocation = Route.LocationAfter(Game.Clock.Time - StartTime);

            if (!newLocation.Equals(Actor.Location))
                Actor.Location = newLocation;

            if (Actor.Location.DistanceTo(Destination()) < 10) Complete();
        }
    }
}