namespace Kartel.Locale.Bing.ApiResponses;

public class Resources
{
    public string Type { get; set; }
    public string Name { get; set; }
    public Point Point { get; set; }
    public Address Address { get; set; }
    public string PhoneNumber { get; set; }
    public string Website { get; set; }
    public string EntityType { get; set; }
    public GeocodePoints[] GeocodePoints { get; set; }
}