using Kartel.Extensions;
using Kartel.Units.Currencies;

namespace Kartel.Entities.Items.Containers;

public class Inventory : Container
{
    public Inventory() : base(20.Liters(), 0.Grams()) { }

    public override CurrencyQuantity BasePrice { get; } = 0.Gbp();
}