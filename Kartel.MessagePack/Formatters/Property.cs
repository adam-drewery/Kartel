using System.Linq.Expressions;
using System.Reflection;

namespace Kartel.MessagePack.Formatters;

public static class Property
{
    private const BindingFlags DeclaredOnlyLookup = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
    
    public static void SetPrivate<T, TValue>(T target, TValue value, Expression<Func<T, TValue>> selector) where T : class
    {
        var setter = GetSetterForProperty(selector);
        setter.Invoke(target, value);
    }
    
    private static Action<T, TValue> GetSetterForProperty<T, TValue>(Expression<Func<T, TValue>> selector) where T : class
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
                return (obj, value) => setter.Invoke(obj, new object?[] { value });
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