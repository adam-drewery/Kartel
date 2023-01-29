using System.Diagnostics.CodeAnalysis;

namespace Kartel.Services;

[SuppressMessage("ReSharper", "UnassignedGetOnlyAutoProperty")]
public class ServiceContainer
{
    public ServiceContainer(
        IPropertyMarketClient propertyMarket, 
        ILocaleClient locale, 
        ILogisticsClient directions, 
        IGeocodingClient geocoder)
    {
        PropertyMarket = propertyMarket;
        Locale = locale;
        Directions = directions;
        Geocoder = geocoder;
    }
    
    public IPropertyMarketClient PropertyMarket { get; }
    
    public ILocaleClient Locale { get; }
        
    public ILogisticsClient Directions { get; }
        
    public IGeocodingClient Geocoder { get; }
}