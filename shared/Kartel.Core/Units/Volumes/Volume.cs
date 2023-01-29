namespace Kartel.Units.Volumes;

public class Volume : IQuantity<VolumeUnit> 
{
    public Volume(VolumeUnit unit, double value)
    {
        Value = value;
        Unit = unit;
    }
        
    public VolumeUnit Unit { get; }
        
    Unit IQuantity.Unit => Unit;

    public double Value { get; }

    public double Liters => Unit.ToLiters(Value);
        
    public static implicit operator double(Volume volume) => volume.Liters;
        
    public static implicit operator Volume(double value) => new(VolumeUnit.Liter, value);
}