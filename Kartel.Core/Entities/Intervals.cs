using System;
using System.Collections.Generic;
using System.Linq;

namespace Kartel.Entities;

public static class Intervals
{
	private static readonly Dictionary<int, DateTime[]> Cache = new();

	public static DateTime[] FromSeconds(int count)
	{
		if (Cache.TryGetValue(count, out var result))
			return result;

		result = CalculateFromSeconds(count).ToArray();
		Cache[count] = result;
		return result;
	}

	private static IEnumerable<DateTime> CalculateFromSeconds(int count)
	{
		var next = DateTime.MinValue;
		var end = next.AddDays(1);

		while (next < end)
		{
			yield return next;
			next = next.AddSeconds(count);
		}
	}
}