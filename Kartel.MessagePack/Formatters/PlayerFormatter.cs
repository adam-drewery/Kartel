using Kartel.Entities;
using Kartel.Environment;
using MessagePack;
using MessagePack.Formatters;

namespace Kartel.MessagePack.Formatters;

public class PlayerFormatter : IMessagePackFormatter<Player>
{
    private readonly PersonFormatter _personFormatter = new();

    public void Serialize(ref MessagePackWriter writer, Player value, MessagePackSerializerOptions options)
    {
        _personFormatter.Serialize(ref writer, value, options);
    }

    public Player Deserialize(ref MessagePackReader reader, MessagePackSerializerOptions options)
    {
        var player = new Player(new Building());
        return PersonFormatter.Populate(ref reader, player, options);
    }
}