using System;
using System.Linq;
using Kartel.Commands;

namespace Kartel.Entities;

public class Need : Observable
{
    private readonly Type _resolutionCommandType;
    private static readonly Random Random = new();

    public static Need Create<TCommand>(string name, double increaseScale, Func<TCommand> resolution)
        where TCommand : Command =>
        new Need(name, increaseScale, resolution, typeof(TCommand));

    public static Need Create<TCommand>(string name, Func<TCommand> resolution)
        where TCommand : Command => 
        Create(name, 1, resolution);

    public Need() { }
    
    private Need(string name, double increaseScale, Func<Command> resolution, Type resolutionCommandType)
    {
        Name = name;
        IncreaseScale = increaseScale;
        Resolution = resolution;
        _resolutionCommandType = resolutionCommandType;
        Value = (byte)Random.Next(byte.MinValue, byte.MaxValue);
    }

    public string Name { get; }

    public double IncreaseScale { get; set; }

    public Func<Command> Resolution { get; }

    public byte Value
    {
        get => Read<byte>();
        set
        {
            var current = Read<byte>();
            Write(value + current >= byte.MaxValue ? byte.MaxValue : value);
        }
    }

    public bool IsUnmet => Value < 128;

    public bool IsCritical => Value < 64;

    public bool IsBeingSatisfied(Person person) => person.CurrentCommand?.GetType() == _resolutionCommandType;

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

        var increaseAmount = numberOfIntervalsPassed * IncreaseScale;

        if (increaseAmount < byte.MaxValue)
            Value += (byte)increaseAmount;
        else
            Value = byte.MaxValue;
    }

    public override string ToString() => $"[{Name}] {Value}";
}