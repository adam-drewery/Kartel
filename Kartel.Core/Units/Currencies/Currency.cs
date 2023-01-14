using System;

namespace Kartel.Units.Currencies;

public class Currency : Unit
{
    public static Currency Gbp { get; } = new('£', "GBP", d => d);
        
    private Currency(char symbol, string name, Func<decimal, decimal> toGbp) : base(name)
    {
        Symbol = symbol;
        ToGbp = toGbp;
    }
        
    public char Symbol { get; }
        
    public Func<decimal, decimal> ToGbp { get; }
}