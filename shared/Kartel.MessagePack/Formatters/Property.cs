using System.Linq.Expressions;
using System.Reflection;

namespace Kartel.MessagePack.Formatters;

/// <summary>Helper class for setting values of private properties.</summary>
public static class Property
{
    private const BindingFlags DeclaredOnlyLookup = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
    
    public static void SetPrivate<T, TValue>(T target, TValue value, Expression<Func<T, TValue>> selector) where T : class
    {
        var setter = GetSetterForProperty(selector);

        if (setter == null)
            throw new InvalidDataException("Failed to find setter for property.");
        
        setter.Invoke(target, value);
    }
    
    private static Action<T, TValue>? GetSetterForProperty<T, TValue>(Expression<Func<T, TValue>> selector) where T : class
    {
        var expression = selector.Body;
        var propertyInfo = expression.NodeType == ExpressionType.MemberAccess ? (PropertyInfo)((MemberExpression)expression).Member : null;

        if (propertyInfo is null)
        {
            return null;
        }

        var setter = GetPropertySetter(propertyInfo);

        return setter;

        static Action<T, TValue> GetPropertySetter(PropertyInfo prop)
        {
            var setter = prop.GetSetMethod(nonPublic: true);
            if (setter is not null)
            {
                return (obj, value) => setter.Invoke(obj, new object[]
 {
     value ?? throw new ArgumentNullException(nameof(value))
 });
            }

            var backingField = prop.DeclaringType?.GetField($"<{prop.Name}>k__BackingField", DeclaredOnlyLookup);
            if (backingField is null)
            {
                throw new InvalidOperationException($"Could not find a way to set {prop.DeclaringType?.FullName}.{prop.Name}. Try adding a private setter.");
            }

            return (obj, value) => backingField.SetValue(obj, value);
        }
    }
}