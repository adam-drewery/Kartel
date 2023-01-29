using System;
using System.Linq;
using System.Threading.Tasks;
using Kartel.Attributes;
using Kartel.Entities;
using Kartel.Environment.Topography;

namespace Kartel.Activities;

[Verb("Move to location", "Moving to location", "Moved to location")]
public class MoveToLocation : Activity
{
    private readonly Person _actor;
    private Task<Route>? _directionsTask;

    public Func<Location?> Destination { get; }

    public Route? Route { get; private set; }

    public MoveToLocation(Person actor, Func<Location?> destination) : base(actor)
    {
        _actor = actor;
        Destination = destination;
    }

    protected override void Update(TimeSpan sinceLastUpdate)
    {
        var destination = Destination() ?? throw new InvalidOperationException("Destination was null");
        
        if (_directionsTask == null)
        {
            var routeDistance = Actor.Location.DistanceTo(destination);
            var duration = TimeSpan.FromSeconds(routeDistance * 1.5); // about 3 m/s

            if (routeDistance < 100)
            {
                _directionsTask = Task.FromResult(
                    Route = new Route
                    {
                        Parts =
                        {
                            new RoutePart(TimeSpan.Zero, _actor.Location),
                            new RoutePart(duration, destination)
                        }
                    });
            }
            else
            {
                var route = new[] { Actor.Location, destination };
                _directionsTask = Services.Directions.WalkingAsync(route);
            }
        }

        if (Route == null)
        {
            if (_directionsTask.IsCompleted)
            {
                Route = _directionsTask.Result;
            }
            else if (_directionsTask.IsFaulted)
            {
                Log.Error(_directionsTask.Exception, "Failed to get directions");
                Complete();
            }
        }
        else
        {
            var newLocation = Route.LocationAfter(Game.Clock.Time - StartTime);
            
            Log.Information("{Actor} ({ID}) moved {Distance:F}m in the past {Duration}ms", 
                Actor, 
                Actor.Id, 
                newLocation.DistanceTo(Actor.Location), 
                sinceLastUpdate.TotalMilliseconds);
            
            Actor.Location = newLocation;
            
            
            if (Actor.Location.DistanceTo(Route.Parts.Last().Location) < 10)
            {
                Complete();
                
                // Skip to the destination if the route didn't take us all the way there
                Actor.Location = destination;
            }
        }
    }
}