using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Serialization;
using Kartel.Entities;
using Kartel.Units;
using Kartel.Units.Currencies;

namespace Kartel.Environment.Topography;

public class Location : GameObject
{
    private bool Equals(Location other)
    {
        return Equals(Game, other.Game) && Latitude.Equals(other.Latitude) && Longitude.Equals(other.Longitude);
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((Location) obj);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            return (Latitude.GetHashCode() * 397) ^ Longitude.GetHashCode();
        }
    }

    public Location() { }
        
    public Location(double latitude, double longitude)
    {
        Latitude = latitude;
        Longitude = longitude;
    }

    public override string ToString() => Address.Lines.Any() 
        ? Address.ToString() 
        : "an unknown location";
        
    [DataMember]
    public double Latitude
    {
        get => Read<double>();
        set => Write(value);
    }

    [DataMember]
    public double Longitude
    {
        get => Read<double>();
        set => Write(value);
    }

    [DataMember]
    public Address Address { get; set; } = new();
        
    protected IDictionary<CurrencyQuantity, IQuantity> BasePrices { get; } = 
        new Dictionary<CurrencyQuantity, IQuantity>();

    public PointF ToPointF() => new((float)Latitude, (float)Longitude);
        
    public double DistanceTo(Location location)
    {
        var d1 = Latitude * (Math.PI / 180.0);
        var num1 = Longitude * (Math.PI / 180.0);
        var d2 = location.Latitude * (Math.PI / 180.0);
        var num2 = location.Longitude * (Math.PI / 180.0) - num1;
        var d3 = Math.Pow(Math.Sin((d2 - d1) / 2.0), 2.0) + Math.Cos(d1) * Math.Cos(d2) * Math.Pow(Math.Sin(num2 / 2.0), 2.0);
    
        return 6376500.0 * (2.0 * Math.Atan2(Math.Sqrt(d3), Math.Sqrt(1.0 - d3)));
    }
}