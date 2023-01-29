using Kartel.Api.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace Kartel.Api.Notifiers;

public class PersonNotifier : CollectionNotifier<PersonHub>
{
    public PersonNotifier(IHubContext<PersonHub> hubContext) : base(hubContext) { }

    public override void Watch(Game game) => Watch(game.Characters);
}