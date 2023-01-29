using System.Threading.Tasks;
using Kartel.Environment.Topography;

namespace Kartel.Services;

public interface IReverseGeocodingClient
{
    Task<Location> ReverseGeocode(Location location);
}