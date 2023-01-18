using System;
using System.Collections;
using System.Collections.Generic;

namespace Kartel.Entities;

public abstract class ObservableQueue
{
    public abstract void EnqueueObject(object item);

    public abstract object DequeueObject();
    
    public abstract void Clear();
}

public class ObservableQueue<T> : ObservableQueue, IReadOnlyCollection<T>, ICollection
{
    public event EventHandler<CollectionChangedArgs> ItemEnqueued;
    
    public event EventHandler<CollectionChangedArgs> ItemDequeued;
    
    public event EventHandler<CollectionChangedArgs> Cleared;

    private readonly Queue<T> _queue = new();

    //clear dequeue enqueue trydequeue

    public override void EnqueueObject(object item) => Enqueue((T)item);

    public override object DequeueObject() => Dequeue();

    public override void Clear()
    {
        _queue.Clear();
        OnCleared(new CollectionChangedArgs(default));
    }

    public void TryPeek(out T result) => _queue.TryPeek(out result);

    public void Enqueue(T item)
    {
        _queue.Enqueue(item);
        OnItemEnqueued(item);
    }

    public T Dequeue()
    {
        var item = _queue.Dequeue();
        OnItemDequeued(item);
        return item;
    }

    public bool TryDequeue(out T result)
    {
        var success = _queue.TryDequeue(out result);
        
        if (success) OnItemDequeued(result);
        
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

    // protected virtual void OnOnCollectionChanged(NotifyCollectionChangedAction action, IList<T> newItems, IList<T> oldItems) 
    //     => OnCollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(action, newItems, oldItems));
    protected virtual void OnItemEnqueued(T item) => ItemEnqueued?.Invoke(this, new CollectionChangedArgs(item));

    protected virtual void OnItemDequeued(T item) => ItemDequeued?.Invoke(this, new CollectionChangedArgs(item));

    protected virtual void OnCleared(CollectionChangedArgs e) => Cleared?.Invoke(this, e);
    
    public T Peek() => _queue.Peek();
}