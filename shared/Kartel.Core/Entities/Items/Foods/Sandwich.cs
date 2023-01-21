using Kartel.Extensions;
using Kartel.Units.Currencies;
using Kartel.Units.Weights;

namespace Kartel.Entities.Items.Foods;

public class Sandwich : Food
{
    public override Weight Weight { get; } = 200.Grams();

    public override CurrencyQuantity BasePrice { get; } = 3.Gbp();
}