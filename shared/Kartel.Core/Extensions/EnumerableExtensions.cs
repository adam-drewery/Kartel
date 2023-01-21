using System;
using System.Collections.Generic;
using System.Linq;

namespace Kartel.Extensions;

public static class EnumerableExtensions
{
    // Prevent concurrency issues
    private static readonly Lazy<Random> LazyRandom = new();

    public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source) => source.OrderBy(_ => Guid.NewGuid());
        
    public static T Random<T>(this IEnumerable<T> source) => source.Shuffle().First();
        
    public static T Random<T>(this ICollection<T> source) => LazyRandom.Value.Element(source);

    public static IEnumerable<IEnumerable<T>> GroupsOf<T>(this IEnumerable<T> source, int groupSize)
    {
        var enumerable = source.ToArray();
        while (enumerable.Any())
        {
            yield return enumerable.Take(groupSize);
            enumerable = enumerable.Skip(groupSize).ToArray();
        }
    }
}   