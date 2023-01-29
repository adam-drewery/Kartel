using System;

namespace Kartel.Entities;

public class Skill
{
    private byte _value;
    private static readonly Random Random = new();
         
    public Skill(string name)
    {
        Name = name;
        _value = (byte) Random.Next(byte.MinValue, byte.MaxValue);
    }

    public string Name { get; }

    public byte Value
    {
        get => _value;
        set => _value = value + _value >= byte.MaxValue
            ? byte.MaxValue
            : value;
    }

    public override string ToString() => $"[{Name}] {Value}";
    
    public static implicit operator byte(Skill skill) => skill.Value;
}