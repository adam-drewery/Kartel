using Kartel.Entities;
using Kartel.Environment;
using MessagePack;
using MessagePack.Formatters;

namespace Kartel.MessagePack.Formatters;

public class PlayerFormatter : IMessagePackFormatter<Player?>
{
    private readonly IGame _game;
    private readonly PersonFormatter _personFormatter;

    public PlayerFormatter(IGame? game)
    {
        _game = game ?? Game.Stub;
        _personFormatter = new PersonFormatter(_game);
    }

    public void Serialize(ref MessagePackWriter writer, Player? value, MessagePackSerializerOptions options)
    {
        _personFormatter.Serialize(ref writer, value, options);
    }

    public Player? Deserialize(ref MessagePackReader reader, MessagePackSerializerOptions options)
    {
        var player = new Player(new House(Game.Stub));
        return PersonFormatter.Populate(_game, ref reader, player, options);
    }
}