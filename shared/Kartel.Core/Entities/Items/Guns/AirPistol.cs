using System;
using Kartel.Extensions;
using Kartel.Units.Currencies;
using Kartel.Units.Weights;

namespace Kartel.Entities.Items.Guns;

public class AirPistol : Handgun
{
    public override Weight Weight { get; } = 1.Kilograms();

    public override CurrencyQuantity? BasePrice { get; } = 100.Gbp();

    public override byte Damage { get; } = 8;

    public override TimeSpan RateOfFire { get; } = TimeSpan.FromMilliseconds(500);

    public override byte MagazineSize { get; } = 20;

    public override byte Reliability { get; } = 64;

    public override byte Accuracy { get; } = 128;
}