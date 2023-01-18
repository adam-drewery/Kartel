namespace Kartel.Units.Currencies;

public class CurrencyQuantity
{
    public CurrencyQuantity(Currency unit, decimal value)
    {
        Unit = unit;
        Value = value;
    }
        
    public Currency Unit { get; }
        
    public decimal Value { get; }

    public override string ToString() => Unit.Symbol + Value.ToString("N2");
}