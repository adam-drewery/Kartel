using Kartel.Commands;
using Kartel.Environment.Topography;
using Kartel.EventArgs;
using MessagePack;
using MessagePack.Formatters;

namespace Kartel.MessagePack.Formatters;

public class PropertyChangedArgsFormatter : IMessagePackFormatter<PropertyChangedArgs>
{
    private readonly CommandFormatter _commandFormatter = new();
    private readonly LocationFormatter _locationFormatter;

    public PropertyChangedArgsFormatter(IGame? game)
    {
        _locationFormatter = new LocationFormatter(game ?? Game.Stub);
    }

    public void Serialize(ref MessagePackWriter writer, PropertyChangedArgs value, MessagePackSerializerOptions options)
    {
        if (value.NewValue == null)
        {
            writer.Write(nameof(Object));
            writer.WriteNil();
        }
        else
            switch (value.NewValue)
            {
                case string @string:
                    writer.Write(nameof(String));
                    writer.Write(@string);
                    break;

                case byte @byte:
                    writer.Write(nameof(Byte));
                    writer.Write(@byte);
                    break;

                case int @int:
                    writer.Write(nameof(Int32));
                    writer.Write(@int);
                    break;

                case Command command:
                    writer.Write(nameof(Command));
                    _commandFormatter.Serialize(ref writer, command, options);
                    break;
             
                case Location location:
                    writer.Write(nameof(location));
                    _locationFormatter.Serialize(ref writer, location, options);
                    break;
                
                default:
                    throw new ArgumentException($"Failed to serialize {value.NewValue} of type {value.NewValue.GetType()}.");
            }

        writer.Write(value.PropertyName);
        writer.Write(value.SourceId.ToString());
        writer.Write((int?)value.QueueChangeType ?? -1);
    }

    public PropertyChangedArgs Deserialize(ref MessagePackReader reader, MessagePackSerializerOptions options)
    {
        var valueType = reader.ReadString();

        object newValue = valueType switch
        {
            nameof(Object) => reader.ReadNil(),
            nameof(String) => reader.ReadString(),
            nameof(Byte) => reader.ReadByte(),
            nameof(Int32) => reader.ReadInt32(),
            nameof(Command) => _commandFormatter.Deserialize(ref reader, options),
            nameof(Location) => _locationFormatter.Deserialize(ref reader, options),
            _ => throw new ArgumentException("Failed to deserialize value.")
        };

        var propertyName = reader.ReadString();
        var sourceId = Guid.Parse(reader.ReadString());
        var queueChangeType = reader.ReadInt32();
        
        return new PropertyChangedArgs(sourceId, propertyName, newValue)
        {
            QueueChangeType = queueChangeType >= 0 ? (QueueChangeType)queueChangeType : null
        };
    }
}