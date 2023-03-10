using System;
using System.Collections;
using System.Collections.Generic;
using Kartel.EventArgs;

namespace Kartel.Observables;

public class ObservableCollection<T> : ObservableCollection, ICollection<T> where T : GameObject
{
    private readonly IDictionary<Guid, T> _items = new Dictionary<Guid, T>();

    public event EventHandler<CollectionChangedArgs>? CollectionChanged;
    
    public event EventHandler? CollectionCleared;

    public override Type ObjectType => typeof(T);

    public T this[Guid id] => _items[id];

    public override object FindObject(Guid id) => this[id];

    public override void AddObject(object item) => Add((T)item);

    public override void RemoveObject(object item) => Remove((T)item);

    public void Add(T item)
    {
        _items.Add(item.Id, item);
        OnCollectionChanged(item, CollectionChangeType.Add);
    }

    public override void Clear()
    {
        _items.Clear();
        OnCollectionCleared();
    }

    public bool Contains(T item) => _items.ContainsKey(item.Id);

    public void CopyTo(T[] array, int arrayIndex) => _items.Values.CopyTo(array, arrayIndex);

    public bool Remove(T item)
    {
        var result = _items.Remove(item.Id);
        OnCollectionChanged(item, CollectionChangeType.Remove);
        return result;
    }

    public int Count => _items.Count;

    public bool IsReadOnly => _items.IsReadOnly;

    public IEnumerator<T> GetEnumerator() => _items.Values.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    protected virtual void OnCollectionChanged(GameObject item, CollectionChangeType changeType) => CollectionChanged?.Invoke(this, new CollectionChangedArgs(item, changeType));

    protected virtual void OnCollectionCleared() => CollectionCleared?.Invoke(this, System.EventArgs.Empty);
}

public abstract class ObservableCollection
{
    public abstract Type ObjectType { get; }
    
    public abstract object FindObject(Guid id);

    public abstract void AddObject(object item);

    public abstract void RemoveObject(object item);

    public abstract void Clear();
}