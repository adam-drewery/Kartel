using Kartel.Web.HubClients.Base;
using Microsoft.AspNetCore.SignalR.Client;

namespace Kartel.Web.HubClients;

public class LogClient : HubClient
{
	public LogClient(HubConnectionBuilder builder, IConfiguration config) : base(builder, config)
	{
		Connection.On<string>("ReceiveLog", s =>
		{
			OnReceiveLog(s);
			return Task.CompletedTask;
		});
	}

	protected virtual void OnReceiveLog(string s) => ReceiveLog?.Invoke(this, s);

	public event EventHandler<string> ReceiveLog;
}