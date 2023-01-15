using Kartel.Api.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace Kartel.Api.Notifiers;

public class PersonNotifier : CollectionNotifier<PersonHub>
{
    public PersonNotifier(IHubContext<PersonHub> hubContext) : base(hubContext) { }

    private Task SendTime(DateTime time) => Clients.All.SendAsync("ReceiveTime", time);

    public override void Watch(Game game) => Watch(game.Characters);
}