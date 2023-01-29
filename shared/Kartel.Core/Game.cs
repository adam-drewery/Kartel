using System;
using Kartel.Entities;
using Kartel.Environment.Topography;
using Kartel.Services;
using Serilog;

namespace Kartel;

public class Game : IGame
{
    private IClock _clock;

    public IClock Clock
    {
        get => _clock;
        set
        {
            _clock.Tick -= ClockOnTick;
            _clock = value;
            _clock.Tick += ClockOnTick;
        }
    }

    private void ClockOnTick(object? sender, System.EventArgs e) => OnTick();

    public ServiceContainer Services { get; }
        
    public Game(
        Func<Game, IPropertyMarketClient> propertyMarket, 
        Func<Game, ILocaleClient> locale, 
        Func<Game, ILogisticsClient> directions, 
        Func<Game, IGeocodingClient> geocoder)
    {
        Services = new ServiceContainer(propertyMarket(this), locale(this), directions(this), geocoder(this));
        
        _clock = new Clock();
        _clock.Tick += ClockOnTick;
        
        Characters = new GameCollection<Person>(this);
        Locations = new GameCollection<Location>(this);
        City.Seed(this);
    }

    public GameCollection<Person> Characters { get; }

    public GameCollection<Location> Locations { get; }

    /// <summary>Used for constructing game objects when there isn't a game context, such as services.</summary>
    public static IGame Stub { get; } = new GameStub();
    
    public void OnTick()
    {
        foreach (var character in Characters)
        {
            try
            {
                character.OnTick();
            }
            catch (Exception e)
            {
                Log.Error(e, "Error encountered processing tick for character {Name} ({ID})", 
                    character.Name,
                    character.Id);
            }
        }
    }

    private class GameStub : IGame
    {
        public ServiceContainer Services => throw new NotImplementedException("Cannot access services in a game stub instance.");
        
        public IClock Clock => throw new NotImplementedException("Cannot access clock in a game stub instance.");
        
        public GameCollection<Person> Characters => throw new NotImplementedException("Cannot access characters in a game stub instance.");
        
        public GameCollection<Location> Locations => throw new NotImplementedException("Cannot access locations in a game stub instance.");
    }
}