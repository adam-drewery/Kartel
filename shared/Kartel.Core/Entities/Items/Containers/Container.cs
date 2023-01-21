using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Kartel.Units.Volumes;
using Kartel.Units.Weights;

namespace Kartel.Entities.Items.Containers;

public abstract class Container : Item, ICollection<IContainable>
{
    protected Container(Volume capacity, double baseWeight)
    {
        Capacity = capacity;
        BaseWeight = baseWeight;
    }

    private readonly ICollection<IContainable> _items = new HashSet<IContainable>();
        
    public virtual Volume Capacity { get; }

    public virtual double BaseWeight { get; }

    public override Weight Weight => BaseWeight + this.Sum(item => item.Weight);

    public int Count => _items.Count;

    public bool IsReadOnly => false;

    public void Add(IContainable item)
    {
        if (CanAdd(item))
            _items.Add(item);
    }

    public bool CanAdd(IContainable item) => Weight + item.Weight <= Capacity;
        
    public bool Remove(IContainable item) => _items.Remove(item);
        
    public void Clear() => _items.Clear();

    public bool Contains(IContainable item) => _items.Contains(item);

    public void CopyTo(IContainable[] array, int arrayIndex) => _items.CopyTo(array, arrayIndex);

    public IEnumerator<IContainable> GetEnumerator() => _items.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable) _items).GetEnumerator();
}