using System.Runtime.Serialization;

namespace Kartel.Entities;

/// <summary>Represents a uni-directional relationship between two people.</summary>
[DataContract]
public class Relationship : GameObject
{
    public Relationship(Person person) : base(person.Game) => Person = person;

    [DataMember]
    public Person Person { get; set; }
    
    [DataMember]
    public byte Trust { get; set; } = byte.MaxValue / 2;
    
    [DataMember]
    public byte Intelligence { get; set; } = byte.MaxValue / 2;
}