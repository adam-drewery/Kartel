using System;
using System.Linq;

namespace Kartel.Entities;

public class Need
{
    private byte _value;
    private static readonly Random Random = new();
        
    public Need() { }
            
    public Need(string name)
    {
        Name = name;
        _value = (byte) Random.Next(byte.MinValue, byte.MaxValue);
    }

    public string Name { get; }
    
    public bool IncreaseScale { get; set; }

    public byte Value
    {
        get => _value;
        set => _value = value + _value >= byte.MaxValue
            ? byte.MaxValue
            : value;
    }

    public void Tick(DateTime gameTime, DateTime lastUpdate)
    {
        // Every 300 seconds we must add 1 point of hunger and fatigue.
        // Each 300th second after midnight is an "interval".
        var intervals = Intervals.FromSeconds(300);
        var lastTick = lastUpdate;
        var currentTick = gameTime;
        int numberOfIntervalsPassed;
            
        if (lastTick.Date == currentTick.Date)
        {
            // If the last update was on the same day as the current update;
            numberOfIntervalsPassed = intervals
                .Where(i => i.TimeOfDay > lastTick.TimeOfDay)
                .Count(i => i.TimeOfDay <= currentTick.TimeOfDay);
        }
        else // Otherwise the last update was a day (or more) previous;
        {
            // Get all previous intervals for today
            numberOfIntervalsPassed = intervals
                .Count(i => i.TimeOfDay <= currentTick.TimeOfDay);

            var previousDate = currentTick.Date.AddDays(-1);
            while (previousDate >= lastTick.Date)
            {
                // If we're on the date of the previous tick, just return intervals after the time
                if (previousDate == lastTick.Date)
                    numberOfIntervalsPassed += intervals.Count(i => i.TimeOfDay > lastTick.TimeOfDay);
                else numberOfIntervalsPassed += intervals.Length;

                previousDate = previousDate.AddDays(-1);
            }
        }

        if (Value + numberOfIntervalsPassed < byte.MaxValue) 
            Value += (byte) numberOfIntervalsPassed;
        else 
            Value = byte.MaxValue;
    }
    
    public override string ToString() => $"[{Name}] {Value}";
}