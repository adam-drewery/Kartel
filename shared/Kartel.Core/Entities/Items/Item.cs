using Kartel.Entities.Items.Containers;
using Kartel.Units.Weights;

namespace Kartel.Entities.Items;

public abstract class Item : IContainable
{
    public abstract Weight Weight { get; }
        
    public Container Container { get; set; }
}