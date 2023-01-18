using Kartel.Commands;
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

public class CommandFormatter : IMessagePackFormatter<Command>
{
    public void Serialize(ref MessagePackWriter writer, Command value, MessagePackSerializerOptions options)
    {
        writer.Write(value.Name.PresentTense);
        writer.Write(value.Name.PastTense);
        writer.Write(value.Name.FutureTense);
    }

    public Command Deserialize(ref MessagePackReader reader, MessagePackSerializerOptions options)
    {
        return new Command
        {
            Name = new VerbName(reader.ReadString(), reader.ReadString(), reader.ReadString())
        };
    }
}