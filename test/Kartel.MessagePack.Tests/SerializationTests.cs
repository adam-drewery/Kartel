using Audacia.Random.Extensions;
using FluentAssertions;
using Kartel.Entities;
using Kartel.Environment.Topography;
using Kartel.Extensions;
using Kartel.Testing;
using MessagePack;
using Xunit;

namespace Kartel.MessagePack.Tests;

public class SerializationTests : GameTests
{
    [Fact]
    public void PersonFormatter()
    {
        var random = new Random();
        var person = new Person(Game)
        {
            FirstName = random.Forename(),
            Surname = random.Surname(),
            Health = (byte)random.Next(0, 255),
            Money = 50.Gbp(),
            Location = new Location(Game, random.NextDouble(), random.NextDouble())
        };
        
        var options = KartelMessagePackSerializerOptions.ForGame(Game);
        var serialized = MessagePackSerializer.Serialize(person, options);

        var deserialized = MessagePackSerializer.Deserialize<Person>(serialized, options);

        deserialized.FirstName.Should().Be(person.FirstName);
    }
    
    [Fact]
    public async Task PlayerFormatter()
    {
        var random = new Random();
        var person = await Player.New(Game);
        
        person.FirstName = random.Forename();
        person.Surname = random.Surname();
        person.Health = (byte)random.Next(0, 255);
        person.Money = 50.Gbp();
        person.Location = new Location(Game, random.NextDouble(), random.NextDouble());

        var options = KartelMessagePackSerializerOptions.ForGame(Game);
        var serialized = MessagePackSerializer.Serialize(person, options);
        var deserialized = MessagePackSerializer.Deserialize<Player>(serialized, options);

        deserialized.FirstName.Should().Be(person.FirstName);
    }
}