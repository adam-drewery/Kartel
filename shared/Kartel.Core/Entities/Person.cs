using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Kartel.Commands;
using Kartel.Entities.Items.Containers;
using Kartel.Entities.Items.Foods;
using Kartel.Entities.Items.MeleeWeapons;
using Kartel.Environment;
using Kartel.Environment.Topography;
using Kartel.Extensions;
using Kartel.Observables;
using Kartel.Units.Currencies;

namespace Kartel.Entities;

[DataContract]
public class Person : GameObject
{
    protected Random Random { get; } = new();

    public static Person Stub { get; } = new(Kartel.Game.Stub);

    public static async Task<Person> New(IGame game)
    {
        var home = await game.Services.PropertyMarket.NewHouse();
        return new Person(home, home);
    }

    public Person(House home) : this(home, home) { }

    protected Person(House home, Location location) : this(home.Game)
    {
        Location = location;
        Money = Random.Next(10000, 20000).Gbp();

        Gender = Random.Enum<Gender>();
        Surname = DataSet.Surnames.Random();

        FirstName = Gender == Gender.Male
            ? DataSet.MaleForenames.Random()
            : DataSet.FemaleForenames.Random();

        Home = home;
        Home.Owner = this;
    }

    public Person(IGame game) : base(game)
    {
        Game.Characters.Add(this);

        Inventory = new Inventory();
        Fists = new Fists();

        Health = byte.MaxValue;
        Needs = new PersonalNeeds(this);

        Needs.PropertyChanged += (_, args) =>
        {
            OnPropertyChanged($"{nameof(Needs)}." + args.PropertyName, args.NewValue);
        };

        Commands.CollectionChanged += (_, args) => OnPropertyChanged(nameof(Commands), args.Item, args.CollectionChangeType);
    }

    public CurrencyQuantity Money
    {
        get => Read<CurrencyQuantity>() ?? CurrencyQuantity.None;
        set => Write(value);
    }

    public string Name => $"{FirstName} {Surname}";

    [DataMember]
    public string? FirstName
    {
        get => Read<string>();
        set => Write(value);
    }

    [DataMember]
    public string? Surname
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
        get => Read<Location>()
               ?? throw new ArgumentException("Actor location cannot be null");
        set
        {
            if (value == null)
                throw new ArgumentException("Actor location cannot be null");

            Write(value);
        }
    }

    public Fists Fists { get; }

    public Inventory Inventory { get; }

    public House? Home
    {
        get => Read<House>();
        set => Write(value);
    }

    [DataMember] public PersonalSkills Skills { get; } = new();

    [DataMember] public PersonalNeeds Needs { get; }

    public Command? CurrentCommand => Commands.Any() ? Commands.Peek() : null;

    // Walking speed in km/h
    public const double WalkSpeed = 8;

    public byte Health
    {
        get => Read<byte>();
        set => Write(value);
    }

    public ObservableCollection<Relationship> Relationships { get; } = new();

    public IEnumerable<Person> Contacts => Relationships.Select(r => r.Person);

    public ICollection<House> Estate { get; } = new HashSet<House>();

    public ObservableQueue<Command> Commands { get; } = new();

    public void Meet(Person person)
    {
        if (Relationships.All(r => r.Person != person))
            Relationships.Add(new Relationship(person));

        if (person.Relationships.All(r => r.Person != this))
            person.Relationships.Add(new Relationship(this));
    }

    public void OnTick()
    {
        Needs.Tick(Game.Clock.Time, Game.Clock.LastUpdate);

        if (CurrentCommand == null || CurrentCommand.IsResolvingNeed)
        {
            var mostCriticalNeed = Needs.Where(need => need.IsCritical).MaxBy(need => need.Value);

            if (mostCriticalNeed != null && !mostCriticalNeed.IsBeingSatisfied(this))
            {
                CurrentCommand?.Cancel();
                Commands.Clear();
                Commands.Enqueue(mostCriticalNeed.Resolution());
                Commands.Single().IsResolvingNeed = true;

                Log.Information("{ActorName} ({ActorID}) is fulfilling need {Need}", Name, Id, mostCriticalNeed.Name);
            }
        }

        if (CurrentCommand != null)
        {
            CurrentCommand.Update();

            if (CurrentCommand.IsComplete)
            {
                var completedCommand = Commands.Dequeue();

                Log.Information("{ActorName} ({ActorID}) has completed command {Command}",
                    Name,
                    Id,
                    completedCommand.GetType().Name);

                var previousEndTime = completedCommand.EndTime;
                if (CurrentCommand == null) return;
                CurrentCommand.Start(previousEndTime);
            }
        }
    }

    public override string ToString() => Name;

    public void Eat(Food food)
    {
        // reduce 1pt per 10g of food
        var amountToReduce = food.Weight.Kilograms * 100;

        if (Needs.Food.Value - amountToReduce < byte.MinValue)
            Needs.Food.Value = 0;
        else
            Needs.Food.Value -= (byte)amountToReduce;
    }
}