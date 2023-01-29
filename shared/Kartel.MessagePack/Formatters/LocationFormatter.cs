using Kartel.Environment.Topography;
using MessagePack;
using MessagePack.Formatters;

namespace Kartel.MessagePack.Formatters;

public class LocationFormatter : IMessagePackFormatter<Location>
{
    public void Serialize(ref MessagePackWriter writer, Location value, MessagePackSerializerOptions options)
    {
        writer.Write(value.Latitude);
        writer.Write(value.Longitude);
    }

    public Location Deserialize(ref MessagePackReader reader, MessagePackSerializerOptions options)
    {
        return new Location
        {
            Latitude = reader.ReadDouble(),
            Longitude = reader.ReadDouble()
        };
    }
}