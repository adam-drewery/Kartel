using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Kartel;

public abstract class TypedList<T> : IEnumerable<T>
{
    public IEnumerator<T> GetEnumerator() =>
        GetType()
            .GetProperties()
            .Where(p => p.PropertyType == typeof(T))
            .Where(p => !p.GetIndexParameters().Any())
            .Select(p => (T)p.GetValue(this))
            .GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}