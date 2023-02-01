using Kartel.Entities;
using Kartel.EventArgs;
using Kartel.MessagePack.Extensions;
using MessagePack;
using MessagePack.Formatters;

namespace Kartel.MessagePack.Formatters;

public class PropertyChangedArgsFormatter : IMessagePackFormatter<PropertyChangedArgs>
{
    private readonly IFormatterResolver _resolver;

    public PropertyChangedArgsFormatter(IGame? game)
    {
        game ??= Game.Stub;
        _resolver = new KartelFormatterResolver(game).CreateComposite();
    }

    public void Serialize(ref MessagePackWriter writer, PropertyChangedArgs value, MessagePackSerializerOptions options)
    {
        if (value.NewValue == null)
        {
            writer.Write(nameof(Object));
            writer.WriteNil();
        }
        else
        {
            var valueType = value.Source?.GetType().GetProperty(value.PropertyName)?.PropertyType
                       ?? value.NewValue.GetType();

            // todo: bit messy, needs expanding
            if (valueType.IsAssignableTo(typeof(ObservableQueue)))
                valueType = valueType.GenericTypeArguments.First();
            
            _resolver.Serialize(ref writer, value.NewValue, options, valueType);
        }

        writer.Write(value.PropertyName);
        writer.Write(value.SourceId.ToString());
        writer.Write((int?)value.QueueChangeType ?? -1);
    }

    public PropertyChangedArgs Deserialize(ref MessagePackReader reader, MessagePackSerializerOptions options)
    {
        var valueType = reader.ReadString();

        object? newValue;
        if (valueType == nameof(Object))
            newValue = reader.ReadNil();
        else
        {
            var targetType = Type.GetType(valueType);

            if (targetType == null)
                throw new TypeLoadException("Failed to load type " + valueType);
            
            newValue = _resolver.Deserialize(ref reader, options, targetType);
        }

        var propertyName = reader.ReadString();
        var sourceId = Guid.Parse(reader.ReadString());
        var queueChangeType = reader.ReadInt32();
        
        return new PropertyChangedArgs(sourceId, propertyName, newValue)
        {
            QueueChangeType = queueChangeType >= 0 ? (QueueChangeType)queueChangeType : null
        };
    }
}