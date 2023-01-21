using Kartel.Environment;
using Kartel.ServiceBase;
using Microsoft.EntityFrameworkCore;
using NetMQ.Sockets;
using Serilog;

namespace Kartel.PropertyMarket;

public class NewHouseFinder : Endpoint<House>
{
	private static readonly Random Random = new();

	protected override Func<ResponseSocket> SocketFactory { get; }

	private House _house;
	public NewHouseFinder(string address)
	{
		SocketFactory = () => new ResponseSocket(address);
	}

	protected override async Task OnWaiting()
	{
		await using var db = new ZooplaDbContext();
		var count = db.Buildings.Count();

		_house = await db.Buildings
			.OrderBy(b => b.Id)
			.Skip(Random.Next(0, count))
			.FirstOrDefaultAsync();
		
		if (_house == null)
			throw new InvalidDataException("No properties in database.");
	}

	protected override Task<House> OnRequest()
	{
		Log.Information("Sending house: {Address}", _house.Address);
		return Task.FromResult(_house);
	}
}