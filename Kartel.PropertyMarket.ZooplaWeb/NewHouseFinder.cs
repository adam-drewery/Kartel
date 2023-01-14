using Kartel.Environment;
using Kartel.ServiceBase;
using Microsoft.EntityFrameworkCore;
using NetMQ.Sockets;
using Serilog;

namespace Kartel.PropertyMarket;

public class NewHouseFinder : Endpoint<Building>
{
	public NewHouseFinder(string address)
	{
		SocketFactory = () => new ResponseSocket(address);
	}

	private static readonly Random Random = new();

	protected override Func<ResponseSocket> SocketFactory { get; }

	private Building _building;

	protected override async Task OnWaiting()
	{
		do
		{
			const int waitInterval = 5000;
			await using var db = new ZooplaDbContext();
			var count = db.Buildings.Count();

			_building = await db.Buildings
				.OrderBy(b => b.Id)
				.Skip(Random.Next(0, count))
				.FirstOrDefaultAsync();

			if (_building != null) return;

			await Task.Delay(waitInterval);
			Log.Warning("No properties in database. Waiting {Interval} ms", waitInterval);
		} 
		while (_building == null);
	}

	protected override Task<Building> OnRequest()
	{
		Log.Information("Sending building: {Address}", _building.Address);
		return Task.FromResult(_building);
	}
}