using System;

namespace Kartel.Units.Volumes;

public class VolumeUnit : Unit
{
    public static VolumeUnit Milileter { get; } = Unit("Milileter", d => d / 1000);
        
    public static VolumeUnit Liter { get; } = Unit("Liter", d => d);
        
    private VolumeUnit(string name, Func<double, double> toLiters) : base(name)
    {
        ToLiters = toLiters;
    }
        
    public Func<double, double> ToLiters { get; }
        
    private static VolumeUnit Unit(string name, Func<double, double> toLiters) => new(name, toLiters);
}