using Audacia.Random;
using Kartel.Environment;
using Kartel.Extensions;
using Serilog;
using Zoopla.ApiClient;
using Timer = System.Timers.Timer;

namespace Kartel.PropertyMarket;

public class ListingCollector : Timer
{
	private static readonly Random Random = new();

	private int _page = 1;

	private string City => Data.Cities[_cityIndex];

	private int _cityIndex = Random.Next(0, Data.Cities.Length - 1);

	private readonly ZooplaClient _zoopla;

	public ListingCollector(string apiKey)
	{
		Elapsed += (_, _) => Collect();
		_zoopla = new ZooplaClient(apiKey);

		// each key is limited to 100 calls per hour
		// That's 1 call per 36 seconds.
		Interval = 40000;
	}

	public async void Collect()
	{
		try
		{
			await TryCollect();
		}
		catch (Exception e)
		{
			Log.Error(e, "Error encountered starting collection");
		}
	}

	private async Task TryCollect()
	{
		if (_cityIndex == Data.Cities.Length)
		{
			_cityIndex = 0;
			_page++;
		}
		else
		{
			_cityIndex++;
		}

		var @params = new PropertyListingsSearchParams
		{
			ListingStatus = ListingStatus.Sale,
			IncludeRented = true,
			IncludeSold = true,
			OrderBy = OrderBy.Age,
			Ordering = Ordering.Ascending,
			MaximumPrice = 250000,
			Area = City,
			Radius = 1000,
			PageSize = 100,
			PageNumber = _page
		};

		PropertyListingsSearchResult listings;
			
		try
		{
			Log.Information("Retrieving page {Page} of property listings in {Region}", _page, @params.Area);
			listings = await _zoopla.PropertyListings(@params);
		}
		catch (HttpRequestException e)
		{
			Log.Error(e, "Error encountered retrieving page {Page} of listings for {Area}", _page, @params.Area);
			Stop();
				
			Log.Information("Delaying next collection by 5 minutes due to request failure");
			await Task.Delay(5 * 60 * 1000);
			return;

		}

		if (listings.Listings == null || !listings.Listings.Any())
		{
			Log.Warning("No listings found");
			return;
		}

		Log.Information("{Count} of {Total} listings found", listings.Listings.Count, listings.ResultCount);

		var result = listings.Listings.ToList();
		var hotels = listings.Listings.Where(l => l.DisplayableAddress.ToLower().Contains("hotel")).ToList();

		if (hotels.Any())
		{
			Log.Information("Excluding {Count} hotels", hotels.Count);
			result = result.Except(hotels).ToList();
		}

		var golfCourses = result.Where(l => l.DisplayableAddress.ToLower().Contains("golf course")).ToList();

		if (golfCourses.Any())
		{
			Log.Information("Excluding {Count} golf courses", golfCourses.Count);
			result = result.Except(golfCourses).ToList();
		}

		var missingPrices = result.Where(l => l.PriceChanges == null || !l.PriceChanges.Any()).ToList();

		if (missingPrices.Any())
		{
			Log.Information("Excluding {Count} without a price history", missingPrices.Count);
			result = result.Except(missingPrices).ToList();
		}

		var missingAddressParts = result.Where(l => string.IsNullOrWhiteSpace(l.StreetName)
		                                            || string.IsNullOrWhiteSpace(l.PostTown)
		                                            || string.IsNullOrWhiteSpace(l.Country))
			.ToList();

		if (missingAddressParts.Any())
		{
			Log.Information("Excluding {Count} without a decent address: {Addresses}", 
				missingAddressParts.Count,
				missingAddressParts.Select(b => b.ToString()));

			result = result.Except(missingAddressParts).ToList();
		}

		var buildings = result
			.Select(building =>
			{
				var house = new House(building.Latitude, building.Longitude)
				{
					ListingPrice = Convert.ToInt32(building.PriceChanges.OrderBy(p => p.Date).Last().Price),
					Floors = building.Floors ?? (short)Random.Next(1, 3),
					Address =
					{
						Lines = new[]
							{
								building.StreetName,
								building.PostTown,
								building.County,
								building.Country
							}
							.Where(s => !string.IsNullOrWhiteSpace(s))
					}
				};
				
				foreach (var _ in Enumerable.Range(0, building.Bathrooms ?? Random.Next(0, 3)))
					house.Bathrooms.Add(new Room(500.CubicFeet()));

				foreach (var _ in Enumerable.Range(0, building.Bedrooms ?? Random.Next(0, 3)))
					house.Bathrooms.Add(new Room(500.CubicFeet()));
                        
				foreach (var _ in Enumerable.Range(0, Random.Next(0, 3)))
					house.Bathrooms.Add(new Room(500.CubicFeet()));
				
				return house;

			}).ToArray();

		await using var db = new ZooplaDbContext();

		var addresses = buildings.Select(b => b.Address.Value);
		var dupes = db.Buildings.Where(b => addresses.Contains(b.Address.Value)).ToList();

		if (dupes.Any())
		{
			Log.Warning("{Count} duplicates found", dupes.Count);

			buildings = buildings
				.Where(b => dupes.All(c => b.Address.Value != c.Address.Value))
				.ToArray();
		}
		else
			Log.Debug("0 duplicates found");

		Log.Debug("Saving {Count} listings", buildings.Length);
		db.Buildings.AddRange(buildings);
		await db.SaveChangesAsync();
	}
}