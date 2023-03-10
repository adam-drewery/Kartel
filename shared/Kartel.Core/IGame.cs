using Kartel.Entities;
using Kartel.Environment.Topography;
using Kartel.Observables;
using Kartel.Services;

namespace Kartel;

public interface IGame
{
    ServiceContainer Services { get; }
    
    IClock Clock { get; }
    
    ObservableCollection<Person> Characters { get; }

    ObservableCollection<Location> Locations { get; }
}