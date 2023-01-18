using System;

namespace Zoopla.ApiClient;

public class ParameterAttribute : Attribute
{
    public ParameterAttribute(string name) => Name = name;

    public string Name { get; set; }
}