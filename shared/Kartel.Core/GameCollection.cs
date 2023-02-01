using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Kartel.EventArgs;

namespace Kartel;

public class GameCollection<T> : ICollection<T> where T : GameObject
{
    private readonly IGame _game;
    private readonly ObservableCollection<T> _items = new();
    
    internal GameCollection(IGame game)
    {
        _game = game;
        _items.CollectionChanged += (sender, args) =>
        {
            var old = args.OldItems?.Cast<GameObject>() ?? Enumerable.Empty<GameObject>();
            var @new = args.NewItems?.OfType<GameObject>() ?? Enumerable.Empty<GameObject>();
            CollectionChanged?.Invoke(sender, new CollectionChangedArgs(old, @new));
        };
    }

    // todo: make this efficient, use a dictionary
    public T this[Guid id] => _items.Single(x => x.Id == id);
        
    public IEnumerator<T> GetEnumerator() => _items.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable) _items).GetEnumerator();

    public bool Remove(T item) => _items.Remove(item);

    public int Count => _items.Count;
        
    public bool IsReadOnly => false;

    public void Add(T item)
    {
        if (item == null) throw new ArgumentNullException();
            
        item.Game = _game;
        _items.Add(item);
    }

    public void Clear() => _items.Clear();

    public bool Contains(T item) => _items.Contains(item);

    public void CopyTo(T[] array, int arrayIndex) => _items.CopyTo(array, arrayIndex);

    public void AddRange(IEnumerable<T> locations)
    {
        foreach (var location in locations)
            _items.Add(location);
    }

    public event EventHandler<CollectionChangedArgs>? CollectionChanged;
}