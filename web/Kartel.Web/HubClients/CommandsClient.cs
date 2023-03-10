using Kartel.Commands;
using Kartel.Web.HubClients.Base;
using Microsoft.AspNetCore.SignalR.Client;

namespace Kartel.Web.HubClients;

public class CommandsClient : CollectionClient<Command>
{
    public CommandsClient(HubConnectionBuilder builder, IConfiguration config) : base(builder, config) { }
}