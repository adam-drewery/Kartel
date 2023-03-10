using System;
using System.Collections;
using System.Collections.Generic;
using Kartel.EventArgs;

namespace Kartel.Observables;

public abstract class ObservableQueue
{
    public abstract void EnqueueObject(object item);

    public abstract object? DequeueObject();

    public abstract void Clear();
}

public sealed class ObservableQueue<T> : ObservableQueue, IReadOnlyCollection<T>, ICollection
    where T : GameObject
{
    public event EventHandler<CollectionChangedArgs>? CollectionChanged;
    
    private readonly Queue<T> _queue = new();
    
    public override void EnqueueObject(object item) => Enqueue((T)item);

    public override object DequeueObject() => Dequeue();

    public override void Clear()
    {
        _queue.Clear();
        OnCollectionCleared();
    }

    public void TryPeek(out T? result) => _queue.TryPeek(out result);

    public void Enqueue(T item)
    {
        _queue.Enqueue(item);
        OnItemEnqueued(item);
    }

    public void Enqueue(IEnumerable<T> items)
    {
        foreach (var item in items)
        {
            _queue.Enqueue(item);
            OnItemEnqueued(item);
        }
    }

    public T Dequeue()
    {
        var item = _queue.Dequeue();
        OnItemDequeued(item);
        return item;
    }

    public bool TryDequeue(out T? result)
    {
        var success = _queue.TryDequeue(out result);

        // todo: how could result be null?
        if (success && result != null) 
            OnItemDequeued(result);

        return success;
    }

    public IEnumerator<T> GetEnumerator()
    {
        return _queue.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable)_queue).GetEnumerator();
    }

    public void CopyTo(Array array, int index)
    {
        ((ICollection)_queue).CopyTo(array, index);
    }

    public int Count => _queue.Count;

    public bool IsSynchronized => ((ICollection)_queue).IsSynchronized;

    public object SyncRoot => ((ICollection)_queue).SyncRoot;

    private void OnItemEnqueued(T item)
    {
        var args = new CollectionChangedArgs(item, CollectionChangeType.Add);
        CollectionChanged?.Invoke(this, args);
    }

    private void OnItemDequeued(T item)
    {
        var args = new CollectionChangedArgs(item, CollectionChangeType.Remove);
        CollectionChanged?.Invoke(this, args);
    }

    private void OnCollectionCleared()
    {
        var args = new CollectionChangedArgs(null, CollectionChangeType.Clear);
        CollectionChanged?.Invoke(this, args);
    }

    public T Peek() => _queue.Peek();
}