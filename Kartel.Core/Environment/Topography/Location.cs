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
        var baseRad = Math.PI * Latitude / 180;
        var targetRad = Math.PI * location.Latitude/ 180;
        var theta = Longitude - location.Longitude;
        var thetaRad = Math.PI * theta / 180;

        var dist =
            Math.Sin(baseRad) 
            * Math.Sin(targetRad) 
            + Math.Cos(baseRad) 
            * Math.Cos(targetRad) 
            * Math.Cos(thetaRad);
            
        dist = Math.Acos(dist);

        dist = dist * 180 / Math.PI;
        dist = dist * 60 * 1.1515;
        return dist * 1.609344;
    }
}