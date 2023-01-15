using System;
using System.Linq;
using Kartel.Commands;
using Kartel.EventArgs;

namespace Kartel.Entities;

public class PersonalNeeds : TypedList<Need>
{
    private readonly Person _person;

    public PersonalNeeds(Person person)
    {
        _person = person;
        Food = Need.Create("Food", () => new GetFood(person));
        Drink = Need.Create("Drink", 2, () => new GetDrink(person));
        Sleep = Need.Create("Sleep", () => new GoToBed(person));
        Drugs = Need.Create("Drugs", 0, () => new TakeDrugs(person));
        Social = Need.Create("Social", 0.2, () => new Socialize(person));

        ConfigureEvents();
    }

    private void ConfigureEvents()
    {
        var properties = GetType()
            .GetProperties()
            .Where(p => p.PropertyType == typeof(Need))
            .Where(p => !p.GetIndexParameters().Any());

        foreach (var property in properties)
        {
            var need = (Need)property.GetValue(this);
            need.PropertyChanged += (sender, args) =>
            {
                OnPropertyChanged((Need)sender, $"{property.Name}.{args.PropertyName}", args.NewValue);
            };
        }
    }

    public Need Food { get; }
        
    public Need Drink { get; }
        
    public Need Sleep { get; }
        
    public Need Drugs { get; }
        
    public Need Social { get; }

    internal void Tick(DateTime gameTime, DateTime lastUpdate)
    {
        foreach (var need in this)
            need.Tick(gameTime, lastUpdate);
    }

    private void OnPropertyChanged(Observable sender, string propertyName, object value)
    {
        // Set the person as the sender as they're the object the subscription happens on
        PropertyChanged?.Invoke(this, new PropertyChangedArgs(_person, propertyName, value));
    }
    
    public event EventHandler<PropertyChangedArgs> PropertyChanged;
}