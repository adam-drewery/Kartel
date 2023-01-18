using System;
using Kartel.Entities;
using Kartel.Environment.Topography;
using Kartel.EventArgs;
using Kartel.Services;

namespace Kartel;

public class Game
{
    public Game()
    {
        Clock = new Clock(this);
        Characters = new GameCollection<Person>(this);
        Locations = new GameCollection<Location>(this);
            
        City.Seed(this);
    }
        
    public IClock Clock { get; set; }

    public ServiceContainer Services { get; }
        
    public Game(params object[] services) : this() => Services = new ServiceContainer(services);

    public GameCollection<Person> Characters { get; }

    public GameCollection<Location> Locations { get; }

    public event EventHandler<ErrorEventArgs> Error;

    public void OnHeartbeat()
    {
        foreach (var character in Characters) 
            character.OnTick();
    }

    internal virtual void OnError(string message, Exception e)
    {
        var args = new ErrorEventArgs(message, e);
        Error?.Invoke(this,args);
    }
}