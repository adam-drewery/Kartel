using System.Collections.Generic;
using Kartel.Entities.Items;
using Kartel.Units.Volumes;

namespace Kartel.Environment;

public class Room
{
    public Room(Volume size, short floor = 1)
    {
        Size = size;
        Floor = floor;
    }

    public Volume Size { get; }
    
    public short Floor { get; }
    
    public ICollection<Item> Contents { get; } = new HashSet<Item>();
}