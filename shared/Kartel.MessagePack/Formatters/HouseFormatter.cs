using Kartel.Environment;
using MessagePack;
using MessagePack.Formatters;

namespace Kartel.MessagePack.Formatters;

public class HouseFormatter : IMessagePackFormatter<House>
{
    private readonly IGame _game;
    private readonly LocationFormatter _locationFormatter;

    public HouseFormatter(IGame? game)
    {
        _game = game ?? Game.Stub;
        _locationFormatter = new LocationFormatter(_game);
    }

    public void Serialize(ref MessagePackWriter writer, House value, MessagePackSerializerOptions options)
    {
        _locationFormatter.Serialize(ref writer, value, options);
    }

    public House Deserialize(ref MessagePackReader reader, MessagePackSerializerOptions options)
    {
        var house = new House(_game)
        {
            Latitude = reader.ReadDouble(),
            Longitude = reader.ReadDouble(),
            Address = { Value = reader.ReadString() }
        };
        
        reader.Depth--;
        return house;
    }
}