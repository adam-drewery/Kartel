using MessagePack;
using MessagePack.Formatters;

namespace Kartel.MessagePack.Extensions;

public static class FormatterResolverExtensions
{
    public static object? Deserialize(this IFormatterResolver resolver, ref MessagePackReader reader, MessagePackSerializerOptions options,
        Type targetType)
    {
        var formatter = (IMessagePackFormatter)resolver.GetFormatterDynamic(targetType);
            
        if (formatter == null)
            throw new TypeLoadException("Failed to load formatter " + targetType);
            
        return formatter.Deserialize(ref reader, options, targetType);
    }
    public static T? Deserialize<T>(this IFormatterResolver resolver, ref MessagePackReader reader, MessagePackSerializerOptions options)
    {
        return (T?)Deserialize(resolver, ref reader, options, typeof(T));
    }
    
    public static void Serialize(this IFormatterResolver resolver, ref MessagePackWriter writer, object value, MessagePackSerializerOptions options,
        Type? targetType = null)
    {
        targetType ??= value.GetType();
        
        var formatter = (IMessagePackFormatter)resolver.GetFormatterDynamic(targetType);
        var typeName = targetType.AssemblyQualifiedName;
            
        writer.Write(typeName);
        formatter.Serialize(ref writer, value, options, targetType);
    }
}