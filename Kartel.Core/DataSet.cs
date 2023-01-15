using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using Kartel.Environment.Topography;

namespace Kartel;

public static class DataSet
{
    private static readonly Dictionary<string, string[][]> Values = new();

    public static City[] Cities { get; } = Set("cities.uk")
        .Select(x => new City(Country.UnitedKingdom, x[0], double.Parse(x[1]), double.Parse(x[2])))
        .ToArray();

    public static string[] Surnames { get; } = Set("dist.all.last").Select(x => x[0]).ToArray();

    public static string[] MaleForenames { get; } = Set("dist.male.first").Select(x => x[0]).ToArray();

    public static string[] FemaleForenames { get; } = Set("dist.female.first").Select(x => x[0]).ToArray();

    private static string TitleCase(string text)
    {
        if (text.Length < 2) return text;
        if (text.StartsWith("Mc", StringComparison.OrdinalIgnoreCase))
            return $"Mc{TitleCase(text.Substring(2))}";

        return text[0].ToString().ToUpperInvariant()
               + text.Substring(1).ToLowerInvariant();
    }

    private static string[][] Set(string resourceName)
    {
        resourceName = $"Kartel.DataSets.{resourceName}";
        if (Values.ContainsKey(resourceName)) return Values[resourceName];
        var assembly = Assembly.GetExecutingAssembly();

        using (var stream = assembly.GetManifestResourceStream(resourceName))
        {
            if (stream == null) throw new MissingManifestResourceException($"{resourceName} not found.");

            using (var reader = new StreamReader(stream))
            {
                var lines = reader.ReadToEnd().Split('\n');
                return Values[resourceName] = lines
                    .Select(line => line
                        .Split(' ')
                        .Select(TitleCase)
                        .Select(s => s.Trim())
                        .ToArray())
                    .ToArray();
            }
        }
    }
}