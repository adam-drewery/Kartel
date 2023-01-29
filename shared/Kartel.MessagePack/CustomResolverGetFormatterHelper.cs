using Kartel.Commands;
using Kartel.Entities;
using Kartel.Environment;
using Kartel.Environment.Topography;
using Kartel.EventArgs;
using Kartel.MessagePack.Formatters;

namespace Kartel.MessagePack;

internal static class CustomResolverGetFormatterHelper
{
    // If type is concrete type, use type-formatter map
    private static readonly Dictionary<Type, Type> FormatterMap = new()
    {
        { typeof(Person), typeof(PersonFormatter) },
        { typeof(Player), typeof(PlayerFormatter) },
        { typeof(Location), typeof(LocationFormatter) },
        { typeof(Shop), typeof(ShopFormatter) },
        { typeof(PropertyChangedArgs), typeof(PropertyChangedArgsFormatter) }
    };

    static CustomResolverGetFormatterHelper()
    {
        var commandTypes = typeof(Command).Assembly
            .GetTypes()
            .Where(t => !t.IsAbstract)
            .Where(t => t.IsAssignableTo(typeof(Command)));
        
        foreach (var type in commandTypes) 
            FormatterMap.Add(type, typeof(CommandFormatter));
    }
    
    internal static object GetFormatter(Type t)
    {
        if (FormatterMap.TryGetValue(t, out var formatter))
            return Activator.CreateInstance(formatter);

        // If type can not get, must return null for fallback mechanism.
        return null;
    }
}