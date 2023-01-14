using System.Threading.Tasks;
using Kartel.Entities;

namespace Kartel.Api.Interfaces.Interfaces;

public interface IPlayerHub
{
	Task<Player> New();
}