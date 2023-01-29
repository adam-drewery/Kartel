using Kartel.Entities.Items.Containers;
using Kartel.Units.Weights;

namespace Kartel.Entities.Items;

public interface IContainable
{
    Weight Weight { get; }
        
    Container? Container { get; set; }
}