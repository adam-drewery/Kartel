using Microsoft.AspNetCore.SignalR;

namespace Kartel.Api.Hubs.Base;

public abstract class BaseHub : Hub
{
	protected readonly Game Game;

	protected BaseHub(Game game) => Game = game;
}