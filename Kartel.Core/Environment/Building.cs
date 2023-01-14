using System.Linq;
using Kartel.Entities;
using Kartel.Environment.Topography;

namespace Kartel.Environment;

public class Building : Location
{
    private Person _owner;

    public Building() : base(0, 0) { }
        
    public Building(double latitude, double longitude) 
        : base(latitude, longitude) { }
        
    public Person Owner
    {
        get => _owner;
        set
        {
            _owner?.Estate.Remove(this);
            _owner = value;
            _owner?.Estate.Add(this);
        }
    }

    public int ListingPrice { get; set; }
        
    public short Bedrooms { get; set; }
        
    public short Bathrooms { get; set; }
        
    public short Floors { get; set; }
        
    public short LivingRooms { get; set; }
        
    public string ExternalId { get; set; }

    public override string ToString() => string.Join(", ", Address.Lines.Distinct().Take(3));
}