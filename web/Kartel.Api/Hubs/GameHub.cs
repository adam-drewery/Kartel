using Kartel.Api.Hubs.Base;

namespace Kartel.Api.Hubs;

public class GameHub : BaseHub
{
	public GameHub(Game game) : base(game) { }
}