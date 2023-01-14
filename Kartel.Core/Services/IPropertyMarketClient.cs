using System.Threading.Tasks;
using Kartel.Environment;

namespace Kartel.Services;

public interface IPropertyMarketClient
{
    Task<Building> NewHouse(int price = 250000);
}