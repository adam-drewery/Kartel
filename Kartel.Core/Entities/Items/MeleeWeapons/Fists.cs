using Kartel.Units.Weights;

namespace Kartel.Entities.Items.MeleeWeapons;

public class Fists : MeleeWeapon
{
    public override Weight Weight { get; } = 0;

    public override byte Damage { get; } = 0;
}