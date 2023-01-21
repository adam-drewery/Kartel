namespace Kartel.Locale.Bing.ApiResponses;

public class GeocodePoints
{
    public string Type { get; set; }
    public double[] Coordinates { get; set; }
    public string CalculationMethod { get; set; }
    public string[] UsageTypes { get; set; }
}