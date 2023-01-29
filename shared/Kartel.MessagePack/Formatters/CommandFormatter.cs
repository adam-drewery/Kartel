using Kartel.Commands;
using Kartel.Entities;
using MessagePack;
using MessagePack.Formatters;

namespace Kartel.MessagePack.Formatters;

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
        return new Command(Person.Stub)
        {
            Name = new VerbName(reader.ReadString(), reader.ReadString(), reader.ReadString())
        };
    }
}