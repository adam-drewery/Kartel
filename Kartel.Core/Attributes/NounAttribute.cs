using System;

namespace Kartel.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class NounAttribute : Attribute
{
    public NounAttribute(string name) => Name = name;
        
    public string Name { get; set; }
}