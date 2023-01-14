namespace Zoopla.Scraping;

public record ListingType
{
    public static readonly ListingType Rent = new("rent"); // todo, not right probably
    public static readonly ListingType Buy = new("for-sale");
    
    public string Key { get; }

    private ListingType(string key) => Key = key;
}