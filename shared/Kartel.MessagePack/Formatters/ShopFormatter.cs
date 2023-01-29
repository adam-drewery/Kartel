using Kartel.Environment;
using MessagePack;
using MessagePack.Formatters;

namespace Kartel.MessagePack.Formatters;

public class ShopFormatter : IMessagePackFormatter<Shop>
{
    private readonly LocationFormatter _locationFormatter = new();
    
    public void Serialize(ref MessagePackWriter writer, Shop value, MessagePackSerializerOptions options)
    {
        _locationFormatter.Serialize(ref writer, value, options);
        writer.Write(value.Name);
    }

    public Shop Deserialize(ref MessagePackReader reader, MessagePackSerializerOptions options)
    {
        var shop = new Shop
        {
            Latitude = reader.ReadDouble(),
            Longitude = reader.ReadDouble(),
            Name = reader.ReadString()
        };
        
        reader.Depth--;
        return shop;
    }
}