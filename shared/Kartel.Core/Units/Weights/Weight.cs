namespace Kartel.Units.Weights;

public class Weight : IQuantity<WeightUnit>
{
    public Weight()
    {
            
    }
        
    public Weight(WeightUnit unit, double value)
    {
        Unit = unit;
        Value = value;            
    }
        
    public WeightUnit Unit { get; }
        
    Unit IQuantity.Unit => Unit;
        
    public double Value { get; }

    public double Kilograms => Unit?.ToKilograms(Value) ?? 0;

    public static implicit operator double(Weight weight) => weight.Kilograms;
        
    public static implicit operator Weight(double value) => new(WeightUnit.Kilogram, value);
}