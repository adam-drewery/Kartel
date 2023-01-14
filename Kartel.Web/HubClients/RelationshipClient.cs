using Kartel.Api.Interfaces;
using Kartel.Entities;
using Kartel.Web.HubClients.Base;
using Microsoft.AspNetCore.SignalR.Client;

namespace Kartel.Web.HubClients;

public class RelationshipClient : EntityClient<Relationship>, IRelationshipHub
{
	public RelationshipClient(HubConnectionBuilder builder, IConfiguration config) : base(builder, config) { }
}