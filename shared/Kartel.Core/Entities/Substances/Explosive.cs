namespace Kartel.Entities.Substances;

public abstract class Explosive : Substance
{
    public abstract byte Power { get; set; }
        
    public abstract byte Volatility { get; set; }

    protected Explosive(double weight) : base(weight) { }
}