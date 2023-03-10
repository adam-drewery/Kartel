using Kartel.Api.Hubs.Base;
using Kartel.Commands;

namespace Kartel.Api.Hubs;

public class CommandsHub : CollectionHub<Command>
{
    public CommandsHub(Game game) : base(game) { }

    protected override Task<List<Command>> LoadData(Guid id)
    {
        return Task.FromResult(Game.Characters[id].Commands.ToList());
    }
}