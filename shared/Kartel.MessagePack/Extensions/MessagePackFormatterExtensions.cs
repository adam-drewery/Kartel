using MessagePack;
using MessagePack.Formatters;

namespace Kartel.MessagePack.Extensions;

public static class MessagePackFormatterExtensions
{
    public static void Serialize(this IMessagePackFormatter formatter, 
        ref MessagePackWriter writer, 
        object value, 
        MessagePackSerializerOptions options,
        Type? valueType = default)
    {
        valueType ??= value.GetType();
        var formatterHelperType = typeof(FormatterHelper<>).MakeGenericType(valueType);
        var formatterHelper = Activator.CreateInstance(formatterHelperType, formatter) as FormatterHelper;
        
        if (formatterHelper == null)
            throw new TypeLoadException("Failed to load formatter helper " + formatterHelperType);
        
        formatterHelper.Serialize(ref writer, value, options);
    }

    public static object? Deserialize(this IMessagePackFormatter formatter, 
        ref MessagePackReader reader, 
        MessagePackSerializerOptions options,
        Type targetType)
    {
        var formatterHelperType = typeof(FormatterHelper<>).MakeGenericType(targetType);
        var formatterHelper = Activator.CreateInstance(formatterHelperType, formatter) as FormatterHelper;
        
        if (formatterHelper == null)
            throw new TypeLoadException("Failed to load formatter helper " + formatterHelperType);
        
        return formatterHelper.Deserialize(ref reader, options);
    }

    // This stuff is necessary to be able to pass a ref param via reflection...
    private abstract class FormatterHelper
    {
        public abstract object? Deserialize(ref MessagePackReader reader, MessagePackSerializerOptions options);

        public abstract void Serialize(ref MessagePackWriter writer, object value, MessagePackSerializerOptions options);
    }

    private class FormatterHelper<T> : FormatterHelper
    {
        private readonly SerializeDelegate _serializeDelegate;
        private readonly DeserializeDelegate _deserializeDelegate;

        private delegate void SerializeDelegate(ref MessagePackWriter writer, T person, MessagePackSerializerOptions options);
        private delegate T DeserializeDelegate(ref MessagePackReader reader, MessagePackSerializerOptions options);

        public FormatterHelper(IMessagePackFormatter formatter)
        {
            _serializeDelegate = (SerializeDelegate)Delegate.CreateDelegate(typeof(SerializeDelegate), formatter, "Serialize");
            _deserializeDelegate = (DeserializeDelegate)Delegate.CreateDelegate(typeof(DeserializeDelegate), formatter, "Deserialize");
        }

        public override object? Deserialize(ref MessagePackReader reader, MessagePackSerializerOptions options) => 
            _deserializeDelegate.Invoke(ref reader, options);

        public override void Serialize(ref MessagePackWriter writer, object value, MessagePackSerializerOptions options) =>
            _serializeDelegate.Invoke(ref writer, (T)value, options);
    }
}