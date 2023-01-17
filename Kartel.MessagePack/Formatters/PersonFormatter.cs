using Kartel.Entities;
using Kartel.Environment.Topography;
using Kartel.Units.Currencies;
using MessagePack;
using MessagePack.Formatters;

namespace Kartel.MessagePack.Formatters;

public class PersonFormatter : IMessagePackFormatter<Person>
{
    public void Serialize(ref MessagePackWriter writer, Person person, MessagePackSerializerOptions options)
    {
        if (person == null)
        {
            writer.WriteNil();
            return;
        }

        writer.Write(person.Id.ToString());
        writer.Write(person.Health);
        writer.Write(person.Surname);
        writer.Write(person.FirstName);
         writer.Write(person.Money.Unit.Name);
        writer.Write((double)person.Money.Value);
        writer.Write(person.Location.Latitude);
        writer.Write(person.Location.Longitude);
        writer.Write(person.Location.Address.Value);

        writer.WriteArrayHeader(person.Needs.Count());

        foreach (var need in person.Needs)
        {
            writer.Write(need.Name);
            writer.Write(need.IncreaseScale);
            writer.Write(need.Value);
        }

        writer.WriteArrayHeader(person.Skills.Count());

        foreach (var skill in person.Skills)
        {
            writer.Write(skill.Name);
            writer.Write(skill.Value);
        }
    }

    public Person Deserialize(ref MessagePackReader reader, MessagePackSerializerOptions options)
    {
        var person = new Person();
        return Populate(ref reader, person, options);
    }
    
    public static T Populate<T>(ref MessagePackReader reader, T person, MessagePackSerializerOptions options) where T : Person
    {
        if (reader.TryReadNil())
        {
            return null;
        }

        options.Security.DepthStep(ref reader);

        person.Id = Guid.Parse(reader.ReadString());
        person.Health = reader.ReadByte();
        person.Surname = reader.ReadString();
        person.FirstName = reader.ReadString();
        person.Money = new CurrencyQuantity(
            Currency.WithName(reader.ReadString()), 
            (decimal)reader.ReadDouble());
        
        person.Location = new Location(reader.ReadDouble(), reader.ReadDouble())
        {
            Address = { Value = reader.ReadString() }
        };

        var count = reader.ReadArrayHeader();

        for (var i = 0; i < count; i++)
        {
            var name = reader.ReadString();
            var target = person.Needs.Single(n => n.Name == name); // todo: slow
            target.IncreaseScale = (float)reader.ReadDouble();
            target.Value = reader.ReadByte();
        }

        count = reader.ReadArrayHeader();

        for (var i = 0; i < count; i++)
        {
            var name = reader.ReadString();
            var target = person.Skills.Single(n => n.Name == name); // todo: slow
            target.Value = reader.ReadByte();
        }

        reader.Depth--;
        return person;
    }
}