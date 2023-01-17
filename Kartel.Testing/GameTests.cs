namespace Kartel.Testing;

public abstract class GameTests
{
    protected Game Game { get; }
    protected MockGeocodingClient Geocoding { get; } = new();
    protected MockLogisticsClient Logistics { get; } = new();
    protected MockPropertyMarketClient PropertyMarket { get; } = new();

    protected GameTests()
    {
        Game = new Game(Geocoding, Logistics, PropertyMarket);
    }
}