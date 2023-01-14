using Kartel.Api.Hubs;
using Microsoft.AspNetCore.SignalR;
using Serilog;

namespace Kartel.Api.Notifiers;

public class GameNotifier : CollectionNotifier<GameHub>
{
	public GameNotifier(IHubContext<GameHub> hubContext) : base(hubContext) { }

	private Task SendTime(DateTime time) => Clients.All.SendAsync("ReceiveTime", time);

	public override void Watch(Game game)
	{
		game.Clock.Tick += (_, _) =>
		{
			try
			{
				SendTime(game.Clock.Time);
			}
			catch (Exception ex)
			{
				Log.Error(ex, "Failed to send time to clients");
			}
		};
	}
}