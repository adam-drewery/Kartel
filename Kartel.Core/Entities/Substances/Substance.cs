namespace Kartel.Entities.Substances;

public abstract class Substance
{
    protected Substance(double weight)
    {
        Weight = weight;
    }

    public double Weight { get; }
}