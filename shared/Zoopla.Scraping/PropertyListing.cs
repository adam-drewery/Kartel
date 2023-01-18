namespace Zoopla.Scraping;

public class PropertyListing
{
    public string Id { get; set; }
    
    public int Price { get; set; }
    
    public short Bedrooms { get; set; }
    
    public short Bathrooms { get; set; }
    
    public short LivingRooms { get; set; }
    
    public double Latitude { get; set; }
    
    public double Longitude { get; set; }

    public Address Address { get; set; } = new();
}