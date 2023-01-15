using Kartel.Api.Hubs.Base;
using Kartel.Entities;

namespace Kartel.Api.Hubs;

public class PersonHub : EntityHub<Person>
{
    public PersonHub(Game game) : base(game) { }

    protected override Task<Person> LoadData(Guid id) => Task.FromResult(Game.Characters[id]);
}