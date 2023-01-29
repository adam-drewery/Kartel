using Kartel.Api.Hubs;
using Microsoft.AspNetCore.SignalR;
using Serilog;

namespace Kartel.Api.Notifiers;

public class LogNotifier : CollectionNotifier<LogHub>
{
	public LogNotifier(IHubContext<LogHub> hubContext) : base(hubContext) { }

	private Task SendLog(string message)
	{
		return Clients.All.SendAsync("ReceiveLog", message);
	}

	public override void Watch(Game game)
	{
		game.Clock.Tick += async (_, _) =>
		{
			try
			{
				await SendLog("Tick");
			}
			catch (Exception ex)
			{
				Log.Error(ex, "Failed to send log notification to clients");
			}
		};
	}
}