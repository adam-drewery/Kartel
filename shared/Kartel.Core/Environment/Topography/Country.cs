using System.Collections.Generic;
using Kartel.Entities.Substances;
using Kartel.Extensions;
using Kartel.Units;
using Kartel.Units.Currencies;

namespace Kartel.Environment.Topography;

public class Country
{
    public static Country UnitedKingdom { get; } = new("United Kingdom")
    {
        BasePrices =
        {
            PriceFor<Cocaine>(50.Gbp(), 1.Gram()),
            PriceFor<Cocaine>(160.Gbp(), (1 / 4).Ounce()),
                
            PriceFor<Cannabis>(20.Gbp(), 2.2.Gram()),
            PriceFor<Cannabis>(70.Gbp(), (1 / 2).Ounce()),
            PriceFor<Cannabis>(160.Gbp(), 1.Ounce()),
                
            PriceFor<Ecstasy>(150.Gbp(), (1 / 4).Ounce()),
            PriceFor<Ecstasy>(500.Gbp(), 1.Ounce()),
                
            PriceFor<Lsd>(5.Gbp(), 200.Microgram()),
            PriceFor<Lsd>(1000.Gbp(), 1.Liter()),
        }
    };

    private Country(string name) => Name = name;

    public string Name { get; }

    public ICollection<BasePrice> BasePrices { get; } = new HashSet<BasePrice>();

    private static BasePrice PriceFor<TItem>(CurrencyQuantity price, IQuantity quantity) =>
        new(typeof(TItem), price, quantity);
}