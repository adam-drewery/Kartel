using System;

namespace Kartel.Units.Volumes;

public class VolumeUnit : Unit
{
    public static VolumeUnit Milliliter { get; } = Unit("Milliliters", d => d / 1000);
        
    public static VolumeUnit Liter { get; } = Unit("Liters", d => d);
    
    public static VolumeUnit CubicFoot { get; } = Unit("Cubic Foot", d => d * 28.31685);
        
    private VolumeUnit(string name, Func<double, double> toLiters) : base(name)
    {
        ToLiters = toLiters;
    }
        
    public Func<double, double> ToLiters { get; }
        
    private static VolumeUnit Unit(string name, Func<double, double> toLiters) => new(name, toLiters);
}