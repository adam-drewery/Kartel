using System.Threading.Tasks;
using Kartel.Environment.Topography;

namespace Kartel.Services;

public interface ILogisticsClient
{
    Task<Route> WalkingAsync(Location[] points);
}