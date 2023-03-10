using Kartel.Environment;
using MessagePack;
using MessagePack.Formatters;

namespace Kartel.MessagePack.Formatters;

public class ShopFormatter : IMessagePackFormatter<Shop>
{
    private readonly IGame _game;
    private readonly LocationFormatter _locationFormatter;

    public ShopFormatter(IGame? game)
    {
        _game = game ?? Game.Stub;
        _locationFormatter = new LocationFormatter(_game);
    }

    public void Serialize(ref MessagePackWriter writer, Shop value, MessagePackSerializerOptions options)
    {
        _locationFormatter.Serialize(ref writer, value, options);
        writer.Write(value.Name);
    }

    public Shop Deserialize(ref MessagePackReader reader, MessagePackSerializerOptions options)
    {
        var shop = new Shop(_game)
        {
            Latitude = reader.ReadDouble(),
            Longitude = reader.ReadDouble(),
            Address = { Value = reader.ReadString() },
            Name = reader.ReadString()
        };
        
        reader.Depth--;
        return shop;
    }
}