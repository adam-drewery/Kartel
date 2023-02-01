using Kartel.Entities;
using Kartel.MessagePack.Extensions;
using MessagePack;
using MessagePack.Formatters;

namespace Kartel.MessagePack.Formatters;

public class RelationshipFormatter : IMessagePackFormatter<Relationship>
{
    private readonly IFormatterResolver _resolver;

    public RelationshipFormatter(IGame? game)
    {
        game ??= Game.Stub;
        _resolver = new KartelFormatterResolver(game).CreateComposite();
    }
    
    public void Serialize(ref MessagePackWriter writer, Relationship value, MessagePackSerializerOptions options)
    {
        _resolver.Serialize(ref writer, value.Person, options);
        writer.Write(value.Trust);
        writer.Write(value.Intelligence);
    }

    public Relationship Deserialize(ref MessagePackReader reader, MessagePackSerializerOptions options)
    {
        var typeName = reader.ReadString();
        var type = Type.GetType(typeName) ?? throw new TypeLoadException("Failed to load type " + typeName);
        
        var person = (Person?)_resolver.Deserialize(ref reader, options, type);

        if (person == null)
            throw new InvalidDataException("Person was null.");
        
        var relationship = new Relationship(person)
        {
            Trust = reader.ReadByte(),
            Intelligence = reader.ReadByte()
        };
        
        reader.Depth--;
        return relationship;
    }
}