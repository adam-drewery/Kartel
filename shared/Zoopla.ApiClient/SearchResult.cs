namespace Zoopla.ApiClient;

public class SearchResult
{
    [Parameter("area_name")]
    public string AreaName { get; set; }

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
}