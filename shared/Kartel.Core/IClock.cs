using System;

namespace Kartel;

public interface IClock
{
    bool Started { get; }
        
    short Interval { get; set; }
        
    DateTime Time { get; }
        
    TimeSpan Delta { get; }
    
    event EventHandler Tick;

    DateTime LastUpdate { get; set; }
        
    float SpeedFactor { get; set; }
        
    void Start();
        
    void Stop();
}