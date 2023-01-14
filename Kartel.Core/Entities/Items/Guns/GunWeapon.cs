using System;

namespace Kartel.Entities.Items.Guns;

public abstract class GunWeapon : Weapon
{
    public abstract TimeSpan RateOfFire { get; }
        
    public abstract byte MagazineSize { get; }
        
    public abstract byte Reliability { get; }
        
    public abstract byte Accuracy { get; }
}