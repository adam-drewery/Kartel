using System.Threading.Tasks;
using Kartel.Environment.Topography;

namespace Kartel.Services;

public interface IGeocodingClient
{
    Task<Location> Geocode(Location location);
        
    Task<Location> ReverseGeocode(Location location);
}