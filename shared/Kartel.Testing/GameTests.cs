namespace Kartel.Testing;

public abstract class GameTests
{
    protected MockClock Clock { get; } = new();
    
    protected Game Game { get; }

    protected MockGeocodingClient Geocoding => (MockGeocodingClient)Game.Services.Geocoder;

    protected MockLogisticsClient Logistics => (MockLogisticsClient)Game.Services.Directions;

    protected MockPropertyMarketClient PropertyMarket => (MockPropertyMarketClient)Game.Services.PropertyMarket;

    protected GameTests()
    {
        Game = new Game(
            g => new MockPropertyMarketClient(g),
            g => new MockLocaleClient(g),
            _ => new MockLogisticsClient(),
            g => new MockGeocodingClient(g)) {Clock = Clock};
    }
}