using Kartel.EventArgs;
using MessagePack;
using MessagePack.Formatters;

namespace Kartel.MessagePack.Formatters;

public class PropertyChangedArgsFormatter : IMessagePackFormatter<PropertyChangedArgs>
{
    private readonly PersonFormatter _personFormatter = new();

    public void Serialize(ref MessagePackWriter writer, PropertyChangedArgs value, MessagePackSerializerOptions options)
    {
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
            
            default:
                throw new ArgumentException($"Failed to serialize {value.NewValue}.");
        }
        
        writer.Write(value.PropertyName);
        writer.Write(value.SourceId.ToString());
    }

    public PropertyChangedArgs Deserialize(ref MessagePackReader reader, MessagePackSerializerOptions options)
    {
        var valueType = reader.ReadString();

        object newValue = valueType switch
        {
            nameof(String) => reader.ReadString(),
            nameof(Byte) => reader.ReadByte(),
            nameof(Int32) => reader.ReadInt32(),
            _ => throw new ArgumentException("Failed to deserialize value.")
        };

        var propertyName = reader.ReadString();
        var sourceId = Guid.Parse(reader.ReadString());
        
        return new PropertyChangedArgs(sourceId, propertyName, newValue);
    }
}