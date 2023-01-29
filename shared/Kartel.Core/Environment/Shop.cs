using System;
using System.Collections.Generic;
using Kartel.Environment.Topography;

namespace Kartel.Environment;

public class Shop : Location
{
    private Shop() : this(Kartel.Game.Stub) { }
    
    public Shop(IGame game) : base(game, 0, 0) { }
    
    public Shop(IGame game, double latitude, double longitude)
        : base(game, latitude, longitude) { }

    public StockType StockType { get; set; }
    
    public string Name { get; set; } = "Food shop";

    public virtual IDictionary<DayOfWeek, OpeningHours> OpeningTimes { get; } = new Dictionary<DayOfWeek, OpeningHours>();

    public override string ToString() => Name + ",  " + Address;
}