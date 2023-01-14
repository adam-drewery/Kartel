using Kartel.Api.Hubs.Base;
using Kartel.Api.Interfaces.Interfaces;
using Kartel.Entities;

namespace Kartel.Api.Hubs;

public class PlayerHub : EntityHub<Player>, IPlayerHub
{
    public PlayerHub(Game game) : base(game) { }

    protected override Task<Player> LoadData(Guid id) => Task.FromResult(Game.Characters[id] as Player);

    public async Task<Player> New()
    {
        var player = await Player.New(Game);
        await Subscribe(player.Id);
        return player;
    }
}