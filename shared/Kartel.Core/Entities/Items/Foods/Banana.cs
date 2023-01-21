using Kartel.Extensions;
using Kartel.Units.Currencies;
using Kartel.Units.Weights;

namespace Kartel.Entities.Items.Foods;

public class Banana : Food
{
    public override Weight Weight { get; } = 120.Grams();

    public override CurrencyQuantity BasePrice { get; } = 0.5m.Gbp();
}