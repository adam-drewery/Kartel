using System;
using System.Collections.Generic;
using System.Linq;

namespace Kartel.Extensions;

public static class RandomExtensions
{
    public static T Enum<T>(this Random random)
    {
        var values = System.Enum.GetValues(typeof(T)).OfType<object>().ToList();
        var value = random.Element(values);
        return (T)value;
    }

    public static T Element<T>(this Random random, ICollection<T> items)
    {
        var index = random.Next(0, items.Count() - 1);
        return items.ElementAt(index);
    }
}