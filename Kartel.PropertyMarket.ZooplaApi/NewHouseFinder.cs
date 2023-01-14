using Kartel.Environment;
using Kartel.ServiceBase;
using Microsoft.EntityFrameworkCore;
using NetMQ.Sockets;
using Serilog;

namespace Kartel.PropertyMarket;

public class NewHouseFinder : Endpoint<Building>
{
	private static readonly Random Random = new();

	protected override Func<ResponseSocket> SocketFactory { get; }

	private Building _building;
	public NewHouseFinder(string address)
	{
		SocketFactory = () => new ResponseSocket(address);
	}

	protected override async Task OnWaiting()
	{
		await using var db = new ZooplaDbContext();
		var count = db.Buildings.Count();

		_building = await db.Buildings
			.OrderBy(b => b.Id)
			.Skip(Random.Next(0, count))
			.FirstOrDefaultAsync();

		if (_building == null)
			throw new InvalidDataException("No properties in database.");
	}

	protected override Task<Building> OnRequest()
	{
		Log.Information("Sending building: {Address}", _building.Address);
		return Task.FromResult(_building);
	}
}