using System;
using System.Collections.Generic;
using System.Linq;

namespace Kartel.Environment.Topography;

public class Route
{
    public TimeSpan Duration() => Parts.Any() ? Parts.Last().ArrivalTime : TimeSpan.Zero;

    public IList<RoutePart> Parts { get; set; } = new List<RoutePart>();
        
    public Location LocationAfter(TimeSpan elapsed)
    {
        return Duration() <= elapsed 
            ? Parts.Last().Location 
            : Parts.First(e => e.ArrivalTime >= elapsed).Location;
    } 
}