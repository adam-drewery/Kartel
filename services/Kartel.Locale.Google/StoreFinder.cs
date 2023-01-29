using Kartel.Environment;
using Kartel.ServiceBase;
using NetMQ.Sockets;
using Newtonsoft.Json.Linq;
using Serilog;
using Location = Kartel.Environment.Topography.Location;

namespace Kartel.Locale.Google;

public class StoreFinder : Endpoint<(Location Location, StockType StockType), Shop>
{
    private readonly string _apiKey;
    private Task _saveTask;
    private readonly HttpClient _http = new();
    
    protected override Func<ResponseSocket> SocketFactory { get; }

    public StoreFinder(string address, string apiKey)
    {
        if (address == null) throw new ArgumentNullException(nameof(address));
        _apiKey = apiKey ?? throw new ArgumentNullException(nameof(apiKey));
        
        SocketFactory = () => new ResponseSocket(address);
    }

    protected override async Task<Shop> Handle((Location Location, StockType StockType) @params)
    {
        var (location, stockType) = @params;

        // Finish the previous save task if there is one
        if (_saveTask != null) await _saveTask;
        
        await using var db = new LocaleDbContext();

        var key = location.Latitude + "+" + location.Longitude;

        var existing = db.ShopLocations
            .Where(x => x.Key == key)
            .Select(x => x.Shop)
            .SingleOrDefault();
        
        if (existing != null)
        {
            Log.Information("Found food store near {Location} in cache", location);
            return existing;
        }
        
        Log.Information("Received request for store selling {StockType}", stockType);
        
        const int radius = 5000;
        
        var type = stockType == StockType.Food ? "supermarket" : throw new ArgumentOutOfRangeException(nameof(@params));

        var url = "https://maps.googleapis.com/maps/api/place/nearbysearch/json" +
                  $"?location={location.Latitude},{location.Longitude}" +
                  $"&radius={radius}" +
                  $"&type={type}" +
                  $"&key={_apiKey}";
        
        var response = await _http.GetAsync(url);
        var json = await response.Content.ReadAsStringAsync();
        var results = JObject.Parse(json).SelectToken("results");

        if (results == null)
            throw new InvalidDataException("No results node in response.");

        var shopLocations = results.Select(token =>
        {
            var latitude = token["geometry"]?["location"]?["lat"];
            var longitude = token["geometry"]?["location"]?["lng"];

            if (latitude == null || longitude == null)
                throw new InvalidDataException("Failed to get location of shop.");
            
            return new ShopLocation
            {
                Key = latitude.Value<string>() + "+" + longitude.Value<string>(),
                Shop = new Shop(Game.Stub)
                {
                    Latitude = latitude.Value<double>(),
                    Longitude = longitude.Value<double>(),
                    Name = token["name"].Value<string>() 
                }
            };
        }).ToList();

        Log.Information("Found {Count} food stores in the area", shopLocations.Count);
        
        db.AddRange(shopLocations);
        _saveTask = db.SaveChangesAsync();
        var shop = shopLocations.First().Shop;

        Log.Information("Returning store {StoreName}", shop.Name);
        return shop;
    }
}