using System.Threading.Tasks;
using Kartel.Environment;
using Kartel.Environment.Topography;

namespace Kartel.Services;

public interface ILocaleClient
{
    Task<Shop> FindStoreAsync((Location Location, StockType BuildingType) @params);
}