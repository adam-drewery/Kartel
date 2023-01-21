using Kartel.Extensions;
using Kartel.Units.Currencies;
using Kartel.Units.Weights;

namespace Kartel.Entities.Items.MeleeWeapons;

public class Fists : MeleeWeapon
{
    public override Weight Weight { get; } = 0;

    public override CurrencyQuantity BasePrice { get; } = 0.Gbp();

    public override byte Damage { get; } = 0;
}