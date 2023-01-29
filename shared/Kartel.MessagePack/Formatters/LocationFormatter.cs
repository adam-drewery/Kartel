using Kartel.Environment.Topography;
using MessagePack;
using MessagePack.Formatters;

namespace Kartel.MessagePack.Formatters;

public class LocationFormatter : IMessagePackFormatter<Location>
{
    private readonly IGame _game;

    public LocationFormatter(IGame? game)
    {
        _game = game ?? Game.Stub;
    }

    public void Serialize(ref MessagePackWriter writer, Location value, MessagePackSerializerOptions options)
    {
        writer.Write(value.Latitude);
        writer.Write(value.Longitude);
        writer.Write(value.Address.Value);
    }

    public Location Deserialize(ref MessagePackReader reader, MessagePackSerializerOptions options)
    {
        return new Location(_game)
        {
            Latitude = reader.ReadDouble(),
            Longitude = reader.ReadDouble(),
            Address = { Value = reader.ReadString() }
        };
    }
}