using Kartel.Environment;
using Kartel.Locale.Bing.ApiResponses;
using Kartel.ServiceBase;
using NetMQ.Sockets;
using Newtonsoft.Json;
using Serilog;
using Location = Kartel.Environment.Topography.Location;

namespace Kartel.Locale.Bing;

public class StoreFinder : Endpoint<(Location Location, StockType BuildingType), Shop>
{
    private readonly HttpClient _http = new();
    private readonly string _apiKey;

    protected override Func<ResponseSocket> SocketFactory { get; }

    public StoreFinder(string address, string apiKey)
    {
        if (address == null) throw new ArgumentNullException(nameof(address));
        _apiKey = apiKey ?? throw new ArgumentNullException(nameof(apiKey));
        
        SocketFactory = () => new ResponseSocket(address);
    }

    protected override async Task<Shop> Handle((Location Location, StockType BuildingType) @params)
    {
        Log.Information("Received request for store selling {BuildingType}", @params.BuildingType);

        var searchRadius = 4000;

        var query = new Dictionary<string, object>
        {
            ["key"] = _apiKey,
            ["o"] = "json",
            ["maxResults"] = "1",
            ["userCircularMapView"] = $"{@params.Location.Latitude},{@params.Location.Longitude},{searchRadius}",
            ["query"] = @params.BuildingType == StockType.Food
                ? "food%20shop"
                : throw new ArgumentOutOfRangeException(nameof(@params.BuildingType))
        };

        var queryString = string.Join('&', query.Select(r => r.Key + '=' + r.Value));
        var json = await _http.GetStringAsync("https://dev.virtualearth.net/REST/v1/LocalSearch?" + queryString);
        var result = JsonConvert.DeserializeObject<RootObject>(json);

        var resource = result.ResourceSets.SelectMany(s => s.Resources).FirstOrDefault();

        if (resource == null)
        {
            Log.Warning("Failed to find store at location {Result} with radius {Radius} meters",
                @params.Location,
                searchRadius);

            return null;
        }

        var geocodePoints = resource.GeocodePoints.First(g => g.Type == "Point");
        var shop = new Shop(Game.Stub)
        {
            StockType = @params.BuildingType,
            Address = { Value = resource.Address.FormattedAddress },
            Latitude = geocodePoints.Coordinates[0],
            Longitude = geocodePoints.Coordinates[1]
        };

        return shop;
    }
}