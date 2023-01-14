using Audacia.Random;
using Kartel.Environment;
using Serilog;
using Zoopla.Scraping;

namespace Kartel.PropertyMarket;

public class ListingCollector : System.Timers.Timer
{
    private static readonly Random Random = new();
    private readonly RegionScraper _regionScraper = new();
    private readonly ListingScraper _listingScraper = new();

    private string City => Data.Cities[_cityIndex];


    private readonly int _cityIndex = Random.Next(0, Data.Cities.Length - 1);

    public ListingCollector()
    {
        Elapsed += async (_, _) => await Collect();
        Interval = 3600000; // every hour

        _listingScraper.ParsingFailed += (_, args) =>
        {
            Log.Warning(args.Exception, "Parsing of property listing failed");
        };
    }

    public async Task Collect()
    {
        try
        {
            await TryCollect();
        }
        catch (Exception e)
        {
            Log.Error(e, "Failed to start collection");
        }
    }

    private async Task TryCollect()
    {
        IList<string> existingIds;
        var tasks = new List<Task>();
        var regions = await _regionScraper.Scrape();

        await using (var db = new ZooplaDbContext())
            existingIds = db.Buildings.Select(b => b.ExternalId).ToList();

        _listingScraper.IgnoreIds = existingIds.ToList();

        foreach (var region in regions)
        {
            try
            {
                tasks.Add(CollectRegion(region));

                await Task.WhenAll(tasks);
            }
            catch (Exception e)
            {
                Log.Error(e, "Failed to collect properties for region \"{Region}\"", region);
            }
        }
    }

    private async Task CollectRegion(string region)
    {
        //var tasks = new List<Task>();

        var propertyTypes = new[]
        {
            PropertyType.SemiDetached,
            PropertyType.Bungalow,
            PropertyType.Detached,
            PropertyType.Flats,
            PropertyType.Terraced
        };

        foreach (var propertyType in propertyTypes)
        {
            // Just do 1 region at a time
            await CollectPropertyTypeForRegion(region, propertyType);
        }
    }

    private async Task CollectPropertyTypeForRegion(string region, PropertyType propertyType)
    {
        short page = 1;
        while (page <= 40)
        {
            Log.Information("Retrieving page {Page} of properties of type {PropertyType} from {Region}", 
                page, 
                propertyType.Key, 
                region);

            var request = new ListingRequest(ListingType.Buy)
            {
                PropertyTypes = { propertyType },
                County = region,
                Radius = 40,
                PageNumber = page
            };

            try
            {
                var listings = await _listingScraper.Scrape(request);

                var buildings = listings
                    .Select(listing =>
                        new Building(listing.Latitude, listing.Longitude)
                        {
                            Address = { Value = listing.Address.Value },
                            Bathrooms = listing.Bathrooms,
                            Bedrooms = listing.Bedrooms,
                            Longitude = listing.Longitude,
                            ListingPrice = listing.Price,
                            LivingRooms = listing.LivingRooms,
                            ExternalId = listing.Id
                        })
                    .ToList();

                await using var db = new ZooplaDbContext();
                db.Buildings.AddRange(buildings);
                await db.SaveChangesAsync();
                Log.Information("{Count} buildings saved", buildings.Count);
            }
            catch (Exception e)
            {
                Log.Error(e, "Error encountered collecting properties of type {PropertyType} for region {Region}",
                    propertyType,
                    region);
            }

            page++;
        }
            
        Log.Information("Collection of properties of type {PropertyType} from {Region} is complete", 
            propertyType.Key, 
            region);
    }
}