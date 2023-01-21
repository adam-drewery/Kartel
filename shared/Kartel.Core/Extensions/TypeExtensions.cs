using System;
using System.Linq;

namespace Kartel.Extensions;

public static class TypeExtensions
{
    public static string PrettyName(this Type type)
    {
        if (type.GetGenericArguments().Length == 0)
        {
            return type.Name;
        }
        var genericArguments = type.GetGenericArguments();
        var typeDefinition = type.Name;
        var unmangledName = typeDefinition.Substring(0, typeDefinition.IndexOf("`", StringComparison.Ordinal));
        return $"{unmangledName}<{string.Join(",", genericArguments.Select(PrettyName))}>";
    }
}