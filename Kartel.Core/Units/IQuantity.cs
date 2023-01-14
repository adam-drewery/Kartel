namespace Kartel.Units;

public interface IQuantity
{
    double Value { get; }
        
    Unit Unit { get; }
}
    
public interface IQuantity<out TUnit> : IQuantity where TUnit : Unit 
{
    new TUnit Unit { get; }
}