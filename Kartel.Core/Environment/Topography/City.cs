using System.Diagnostics;

namespace Kartel.Environment.Topography;

public class City : Location
{
    public City(Country country, string name, double latitude, double longitude) 
        : base(latitude, longitude)
    {
        Country = country;
        Name = name;
    }

    public Country Country { get; }
        
    public string Name { get; }

    /// <summary>Adds the default cities to the gamestate of the specified game.</summary>
    internal static void Seed(Game game)
    {
        foreach (var city in DataSet.Cities)
        {
            Debug.WriteLine("seed: " + city.Name);
        }
    }
}