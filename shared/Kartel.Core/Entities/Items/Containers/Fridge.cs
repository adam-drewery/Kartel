using Kartel.Extensions;
using Kartel.Units.Currencies;

namespace Kartel.Entities.Items.Containers;

public class Fridge : Container
{
    public Fridge() : base(20.CubicFeet(), 100.Kilograms()) { }

    public override CurrencyQuantity BasePrice { get; } = 300.Gbp();
}