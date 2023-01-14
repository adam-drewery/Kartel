using System;

namespace Kartel.Environment.Topography;

public class RoutePart
{
	public RoutePart(TimeSpan arrivalTime, Location location)
	{
		ArrivalTime = arrivalTime;
		Location = location;
	}
        
	public TimeSpan ArrivalTime { get; set; }

	public Location Location { get; set; }
}