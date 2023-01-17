using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Kartel.Commands;
using Kartel.Entities.Items.Containers;
using Kartel.Entities.Items.MeleeWeapons;
using Kartel.Environment;
using Kartel.Environment.Topography;
using Kartel.Extensions;
using Kartel.Units.Currencies;

namespace Kartel.Entities;

[DataContract]
public class Person : GameObject
{
    protected Random Random { get; } = new();

    public static async Task<Person> New(Game game)
    {
        var home = await game.Services.PropertyMarket.NewHouse();
        return new Person(home, home);
    }

    public Person(Building home) : this(home, home) { }

    protected Person(Building home, Location location) : this()
    {
        Location = location;
        Money = Random.Next(10000, 20000).Gbp();
        Inventory = new Inventory(this);

        Fists = new Fists();
        Gender = new Random().Enum<Gender>();
        Surname = DataSet.Surnames.Random();
        
        FirstName = Gender == Gender.Male
            ? DataSet.MaleForenames.Random()
            : DataSet.FemaleForenames.Random();
        
        Home = home;
        Home.Owner = this;
    }

    public Person()
    {
        Health = byte.MaxValue;
        Needs = new PersonalNeeds(this);
        Relationships = new GameCollection<Relationship>(Game);

        Needs.PropertyChanged += (_, args) =>
        {
            OnPropertyChanged($"{nameof(Needs)}." + args.PropertyName, args.NewValue);
        };
    }
    
    public CurrencyQuantity Money
    {
        get => Read<CurrencyQuantity>();
        set => Write(value);
    }

    public string Name => $"{FirstName} {Surname}";

    [DataMember]
    public string FirstName
    {
        get => Read<string>();
        set => Write(value);
    }

    [DataMember]
    public string Surname
    {
        get => Read<string>();
        set => Write(value);
    }

    [DataMember]
    public Gender Gender
    {
        get => Read<Gender>();
        set => Write(value);
    }

    [DataMember]
    public Location Location
    {
        get => Read<Location>();
        set => Write(value);
    }

    public Fists Fists { get; }

    public Inventory Inventory { get; }
    
    public Building Home
    {
        get => Read<Building>();
        set => Write(value);
    }

    [DataMember]
    public PersonalSkills Skills { get; } = new();

    [DataMember]
    public PersonalNeeds Needs { get; }

    public Command CurrentCommand => Commands.Any() ? Commands.Peek() : null;

    // Walking speed in km/h
    public const double WalkSpeed = 8;

    public byte Health
    {
        get => Read<byte>();
        set => Write(value);
    }
    
    public GameCollection<Relationship> Relationships { get; }

    public IEnumerable<Person> Contacts => Relationships.Select(r => r.Person);
        
    public ICollection<Building> Estate { get; } = new HashSet<Building>();

    public Queue<Command> Commands { get; } = new();

    public void Meet(Person person)
    {
        if (Relationships.All(r => r.Person != person))
            Relationships.Add(new Relationship { Person = person });

        if (person.Relationships.All(r => r.Person != this))
            person.Relationships.Add(new Relationship { Person = this });
    }

    public void OnTick()
    {
        Needs.Tick(Game.Clock.Time, Game.Clock.LastUpdate);

        var mostCriticalNeed = Needs.Where(need => need.IsCritical).MaxBy(need => need.Value);

        if (mostCriticalNeed != null && !mostCriticalNeed.IsBeingSatisfied(this))
        {
            CurrentCommand?.Cancel();
            Commands.Clear();
            Commands.Enqueue(mostCriticalNeed.Resolution());
        }

        if (CurrentCommand == null) return;
        
        CurrentCommand.Update();
        
        while (CurrentCommand is { Complete: true })
        {
            var completedCommand = Commands.Dequeue();
            
            Console.WriteLine("{0} completing command {1}", this, completedCommand.GetType().Name);
            var previousEndTime = completedCommand.EndTime;
            if (Commands.Any()) break;
                
            CurrentCommand.Update();

            if (!CurrentCommand.Complete) return;

            CurrentCommand?.Start(previousEndTime);
        }
    }

    public override string ToString() => Name;
}