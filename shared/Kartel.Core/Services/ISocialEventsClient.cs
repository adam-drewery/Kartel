using System.Threading.Tasks;
using Kartel.Environment;
using Kartel.Environment.Topography;

namespace Kartel.Services;

public interface ISocialEventsClient
{
    // TODO: https://github.com/Skiddle/web-api
    Task<SocialEvent> ClosestTo(Location location);
}