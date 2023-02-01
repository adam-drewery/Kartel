using System.Collections.Generic;
using System.Linq;
using Kartel.Entities;
using Kartel.Entities.Items.Containers;
using Kartel.Environment.Topography;
using Kartel.Extensions;

namespace Kartel.Environment;

public class House : Location
{
    private Person? _owner;

    // todo: needed for EF, remove when possible
    private House() : this(Kartel.Game.Stub) { }
    
    public House(IGame game) : base(game, 0, 0) { }
    
    public House(IGame game, double latitude, double longitude) 
        : base(game, latitude, longitude) { }
        
    public Person? Owner
    {
        get => _owner;
        set
        {
            _owner?.Estate.Remove(this);
            _owner = value;
            _owner?.Estate.Add(this);
        }
    }

    public IEnumerable<Room> Rooms => Bedrooms
        .Concat(Bathrooms)
        .Concat(LivingRooms)
        .Concat(Kitchens);

    public ICollection<Room> Bedrooms { get; } = new List<Room>();

    public ICollection<Room> Bathrooms { get; } = new List<Room>();

    public ICollection<Room> LivingRooms { get; } = new List<Room>();
    
    // Give every house a kitchen by default
    public ICollection<Room> Kitchens { get; } = new List<Room>
    {
        new(500.CubicFeet()){Contents = { new Fridge() }}
    };
    
    public int ListingPrice { get; set; }
    
    public short Floors { get; set; }

    public string? ExternalId { get; set; }

    public override string ToString() => string.Join(", ", Address.Lines.Distinct().Take(3));
}