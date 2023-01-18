using System;
using Kartel.Environment.Topography;

namespace Kartel.Environment;

public class SocialEvent
{
    public SocialEvent(string name, Location location, DateTime startTime, DateTime endTime)
    {
        Name = name;
        Location = location;
        StartTime = startTime;
        EndTime = endTime;
    }
        
    public string Name { get; set; }
        
    public Location Location { get; }
        
    public DateTime StartTime { get; }
        
    public DateTime EndTime { get; }
}