using Kartel.Api.Interfaces.Interfaces;
using Kartel.Entities;
using Kartel.Web.HubClients.Base;
using Microsoft.AspNetCore.SignalR.Client;

namespace Kartel.Web.HubClients;

public class PlayerClient : EntityClient<Player>, IPlayerHub
{
	public PlayerClient(HubConnectionBuilder builder, IConfiguration config) : base(builder, config) { }

	public async Task<Player> New()
	{
		return await Connection.InvokeAsync<Player>(nameof(New));
	}
}