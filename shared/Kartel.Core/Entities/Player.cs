using System.Linq;
using System.Threading.Tasks;
using Kartel.Environment;
using Kartel.Environment.Topography;

namespace Kartel.Entities;

public class Player : Person
{
    /// <summary>Create an empty new player with a home and location.</summary>
    public Player(House home, Location location) : base(home, location) { }

    /// <summary>Create an empty new player with a home.</summary>
    public Player(House home) : this(home, home) { }
    
    /// <summary>Create a new player and give them a home and some contacts/associates etc.</summary>
    public new static async Task<Player> New(IGame game)
    {
        var home = await game.Services.PropertyMarket.NewHouse();
        
        var associates = Enumerable.Range(0, 9)
            .Select(_ => Person.New(game));

        var player =  new Player(home);
        
        // Add some relationships
        foreach (var associate in associates) // todo: slow
            player.Meet(await associate);

        return player;
    }
}