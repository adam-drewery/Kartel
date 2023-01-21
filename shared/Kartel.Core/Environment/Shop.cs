using Kartel.Environment.Topography;

namespace Kartel.Environment;

public class Shop : Location
{
    public Shop() : base(0, 0) { }
    
    public Shop(double latitude, double longitude) 
        : base(latitude, longitude) { }

    public StockType StockType { get; set; }
    
    public string Name { get; set; } = "Food shop";

    public override string ToString() => Name + ",  " + Address;
}