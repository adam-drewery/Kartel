using System;

namespace Kartel.Extensions;

public static class ShortExtensions
{
    public static decimal Percent(this byte value, int decimalPlaces)
    {
        const decimal one = byte.MaxValue / 100m;
        var result = value / one;
        return Math.Round(result, decimalPlaces);
    }
}