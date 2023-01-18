using System;
using System.Collections.Generic;
using System.Linq;

namespace Kartel.Entities;

public static class Intervals
{
	private static readonly Dictionary<int, DateTime[]> SecondsCache = new();
	private static readonly Dictionary<int, DateTime[]> MillisecondsCache = new();

	public static IEnumerable<DateTime> ForRange(this ICollection<DateTime> source, DateTime lower, DateTime upper)
	{
		List<DateTime> intervalsPassed;

		if (lower.Date == upper.Date)
		{
			// If the last update was on the same day as the current update;
			return source
				.Where(i => i.TimeOfDay > lower.TimeOfDay)
				.Where(i => i.TimeOfDay <= upper.TimeOfDay);
		}
		else // Otherwise the last update was a day (or more) previous;
		{
			// Get all previous intervals for today
			intervalsPassed = source
				.Where(i => i.TimeOfDay <= upper.TimeOfDay)
				.ToList();

			var previousDate = upper.Date.AddDays(-1);
			while (previousDate >= lower.Date)
			{
				// If we're on the date of the previous tick, just return intervals after the time
				if (previousDate == lower.Date)
					intervalsPassed.AddRange(source.Where(i => i.TimeOfDay > lower.TimeOfDay));
				else intervalsPassed.AddRange(source);

				previousDate = previousDate.AddDays(-1);
			}
		}

		return intervalsPassed;
	}
	
	public static DateTime[] FromSeconds(int count)
	{
		if (SecondsCache.TryGetValue(count, out var result))
			return result;

		result = CalculateFromSeconds(count).ToArray();
		SecondsCache[count] = result;
		return result;
	}
	
	public static DateTime[] FromMilliseconds(int count)
	{
		if (MillisecondsCache.TryGetValue(count, out var result))
			return result;

		result = CalculateFromMillis(count).ToArray();
		MillisecondsCache[count] = result;
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
	private static IEnumerable<DateTime> CalculateFromMillis(int count)
	{
		var next = DateTime.MinValue;
		var end = next.AddDays(1);

		while (next < end)
		{
			yield return next;
			next = next.AddMilliseconds(count);
		}
	}
}