using Kartel.Extensions;

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

    public static CurrencyQuantity None { get; } = 0.Gbp();

    public override string ToString() => Unit.Symbol + Value.ToString("N2");
}