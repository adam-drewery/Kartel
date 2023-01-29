using Kartel.Entities;
using Kartel.Environment.Topography;
using Kartel.Services;

namespace Kartel;

public interface IGame
{
    ServiceContainer Services { get; }
    
    IClock Clock { get; }
    
    GameCollection<Person> Characters { get; }

    GameCollection<Location> Locations { get; }
}