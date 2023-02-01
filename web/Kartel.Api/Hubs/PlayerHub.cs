using Kartel.Api.Hubs.Base;
using Kartel.Api.Interfaces;
using Kartel.Entities;

namespace Kartel.Api.Hubs;

public class PlayerHub : EntityHub<Player>, IPlayerHub
{
    public PlayerHub(Game game) : base(game) { }

    protected override Task<Player> LoadData(Guid id) => Task.FromResult((Player)Game.Characters[id]);

    public async Task<Player> New()
    {
        var player = await Player.New(Game);
        await Subscribe(player.Id);
        return player;
    }
}