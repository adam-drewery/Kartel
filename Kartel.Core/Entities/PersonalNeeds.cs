using System;

namespace Kartel.Entities;

public class PersonalNeeds : TypedList<Need>
{
    public Need Food { get; } = new("Food");
        
    public Need Drink { get; } = new("Drink");
        
    public Need Sleep { get; } = new("Sleep");
        
    public Need Drugs { get; } = new("Drugs");
        
    public Need Social { get; } = new("Social");

    internal void Tick(DateTime gameTime, DateTime lastUpdate)
    {
        foreach (var need in this) 
            need.Tick(gameTime, lastUpdate);
    }
}