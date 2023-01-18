using Kartel.Entities;
using Kartel.Web.HubClients.Base;
using Microsoft.AspNetCore.SignalR.Client;

namespace Kartel.Web.HubClients;

public class RelationshipsClient : CollectionClient<Relationship>
{
	public RelationshipsClient(HubConnectionBuilder builder, IConfiguration config) : base(builder, config) { }
}