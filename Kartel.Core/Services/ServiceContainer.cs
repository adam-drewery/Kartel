using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Kartel.Services;

[SuppressMessage("ReSharper", "UnassignedGetOnlyAutoProperty")]
public class ServiceContainer
{
    public ServiceContainer(params object[] services)
    {
        foreach (var service in services)
        {
            var property = GetType().GetProperties()
                .Single(p => p.PropertyType.IsInstanceOfType(service));
                
            property.SetValue(this, service);
        }
    }
        
    public IPropertyMarketClient PropertyMarket { get; set; }
        
    public ILogisticsClient Directions { get; set; }
        
    public IGeocodingClient Geocoder { get; set; }
        
    public ISocialEventsClient SocialEvents { get; set; }
}