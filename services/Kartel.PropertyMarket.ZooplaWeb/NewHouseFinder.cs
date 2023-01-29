using Kartel.Environment;
using Kartel.ServiceBase;
using Microsoft.EntityFrameworkCore;
using NetMQ.Sockets;
using Serilog;

namespace Kartel.PropertyMarket.ZooplaWeb;

public class NewHouseFinder : Endpoint<House>
{
	public NewHouseFinder(string address)
	{
		SocketFactory = () => new ResponseSocket(address);
	}

	private static readonly Random Random = new();

	protected override Func<ResponseSocket> SocketFactory { get; }

	private House _house;

	protected override async Task OnWaiting()
	{
		do
		{
			const int waitInterval = 5000;
			await using var db = new ZooplaDbContext();
			var count = db.Buildings.Count();

			_house = await db.Buildings
				.OrderBy(b => b.Id)
				.Skip(Random.Next(0, count))
				.FirstOrDefaultAsync();

			if (_house != null) return;

			await Task.Delay(waitInterval);
			Log.Warning("No properties in database. Waiting {Interval} ms", waitInterval);
		} 
		while (_house == null);
	}

	protected override Task<House> OnRequest()
	{
		Log.Information("Sending house: {Address}", _house.Address);
		return Task.FromResult(_house);
	}
}