namespace Zoopla.Scraping;

public record PropertyType(string Key, bool IsHouse)
{
    public static readonly PropertyType Flats = new("flats", false);
    
    public static readonly PropertyType SemiDetached = new("semi_detached", true);
    
    public static readonly PropertyType Bungalow = new("bungalow", true);
    
    public static readonly PropertyType Terraced = new("terraced", true);
    
    public static readonly PropertyType Detached = new("detached", true);
}