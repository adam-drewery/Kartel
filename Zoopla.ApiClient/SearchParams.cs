namespace Zoopla.ApiClient;

public class SearchParams
{
    [Parameter("area")]
    public string Area { get; set; }

    [Parameter("street")]
    public string Street { get; set; }

    [Parameter("town")]
    public string Town { get; set; }

    [Parameter("postcode")]
    public string Postcode { get; set; }

    [Parameter("county")]
    public string County { get; set; }

    [Parameter("country")]
    public string Country { get; set; }

    [Parameter("latitude")]
    public double Latitude { get; set; }

    [Parameter("longitude")]
    public double Longitude { get; set; }
}