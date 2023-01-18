using System;
using System.Linq;
using Kartel.Commands;

namespace Kartel.Entities;

public class Need : Observable
{
    private readonly Type _resolutionCommandType;
    private static readonly Random Random = new();

    public static Need Create<TCommand>(string name, double increaseScale, Func<TCommand> resolution)
        where TCommand : Command => new(name, increaseScale, resolution, typeof(TCommand));

    public static Need Create<TCommand>(string name, Func<TCommand> resolution)
        where TCommand : Command => 
        Create(name, 1, resolution);

    public Need(string name, double increaseScale, Func<Command> resolution, Type resolutionCommandType)
    {
        Name = name;
        IncreaseScale = increaseScale;
        Resolution = resolution;
        _resolutionCommandType = resolutionCommandType;
    }

    public string Name { get; }

    public double IncreaseScale { get; set; }

    public Func<Command> Resolution { get; }

    public byte Value
    {
        get => Read<byte>();
        set => Write(value);
    }

    public bool IsUnmet => Value > 128;

    public bool IsCritical => Value > 192;

    public bool IsBeingSatisfied(Person person) => person.CurrentCommand?.GetType() == _resolutionCommandType;

    public void Tick(DateTime gameTime, DateTime lastUpdate)
    {
        var intervals = Intervals
            .FromSeconds(300)
            .ForRange(lastUpdate, gameTime)
            .Count();
        
        var increaseAmount = intervals * IncreaseScale;

        if (increaseAmount < byte.MaxValue)
            Value += (byte)increaseAmount;
        else
            Value = byte.MaxValue;
    }

    public override string ToString() => $"[{Name}] {Value}";
}