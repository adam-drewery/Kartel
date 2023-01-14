using System;

namespace Kartel;

public interface IClock
{
    bool Started { get; }
        
    double Interval { get; set; }
        
    DateTime Time { get; }
        
    TimeSpan Delta { get; }
        
    event EventHandler Tick;
        
    double MinimumTickSpeed { get; set; }
        
    DateTime LastUpdate { get; set; }
        
    float SpeedFactor { get; set; }
        
    void Start();
        
    void Stop();
}