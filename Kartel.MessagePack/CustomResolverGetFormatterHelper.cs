using Kartel.Entities;
using Kartel.EventArgs;
using Kartel.MessagePack.Formatters;

namespace Kartel.MessagePack;

internal static class CustomResolverGetFormatterHelper
{
    // If type is concrete type, use type-formatter map
    static readonly Dictionary<Type, Type> FormatterMap = new()
    {
        { typeof(Person), typeof(PersonFormatter) },
        { typeof(Player), typeof(PlayerFormatter) },
        { typeof(PropertyChangedArgs), typeof(PropertyChangedArgsFormatter) }
    };

    internal static object GetFormatter(Type t)
    {
        if (FormatterMap.TryGetValue(t, out var formatter))
            return Activator.CreateInstance(formatter);

        // If type can not get, must return null for fallback mechanism.
        return null;
    }
}