using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Kartel.Entities;

/// <summary>The postal address of a location.</summary>
public class Address
{
    public string Value { get; set; } = string.Empty;

    [IgnoreDataMember]
    public IEnumerable<string> Lines
    {
        get => Value.Split(new[]{'\r','\n'}, StringSplitOptions.RemoveEmptyEntries);
        set => Value = string.Join(System.Environment.NewLine, value);
    }

    public override string ToString() => string.Join(", ", Lines
        .Where(l =>!string.IsNullOrWhiteSpace(l))
        .Distinct());
        
    public string ToString(int lines) => string.Join(", ", Lines
        .Where(l =>!string.IsNullOrWhiteSpace(l))
        .Take(lines)
        .Distinct());
}