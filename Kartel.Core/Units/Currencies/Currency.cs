using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Kartel.Units.Currencies;

public class Currency : Unit
{
    public static Currency Gbp { get; } = new('£', "GBP");

    private Currency(char symbol, string name) : base(name)
    {
        Symbol = symbol;
    }
        
    public char Symbol { get; }

    public static Currency WithName(string name) => All.Single(c => c.Name == name);

    private static readonly ICollection<Currency> All =
        typeof(Currency)
            .GetProperties(BindingFlags.Public | BindingFlags.Static)
            .Where(p => p.PropertyType == typeof(Currency))
            .Where(p => !p.GetIndexParameters().Any())
            .Select(p => (Currency)p.GetValue(null))
            .ToHashSet();

}