namespace Kartel.Locale.Bing.ApiResponses;

public class RootObject
{
    public string AuthenticationResultCode { get; set; }
    public string BrandLogoUri { get; set; }
    public string Copyright { get; set; }
    public ResourceSets[] ResourceSets { get; set; }
    public int StatusCode { get; set; }
    public string StatusDescription { get; set; }
    public string TraceId { get; set; }
}