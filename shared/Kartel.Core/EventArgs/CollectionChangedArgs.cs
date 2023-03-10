using System;
using Kartel.Observables;

namespace Kartel.EventArgs;

public class CollectionChangedArgs
{
	public CollectionChangedArgs(GameObject? item, CollectionChangeType collectionChangeType)
	{
		ItemId = item?.Id;
		Item = item;
		CollectionChangeType = collectionChangeType;
	}

	public Guid? ItemId { get; }

	public GameObject? Item { get; }
	
	public CollectionChangeType CollectionChangeType { get; }

	public void ApplyTo(ObservableCollection target)
	{
		if (Item == null) throw new InvalidOperationException("Can't add or remove a null item.");
		
		if (CollectionChangeType == CollectionChangeType.Add) target.AddObject(Item);
		else if (CollectionChangeType == CollectionChangeType.Remove) target.RemoveObject(Item);
	}
	
	public void ApplyTo(ObservableQueue queue)
	{
		if (Item == null) throw new InvalidOperationException("Can't add or remove a null item.");
		
		if (CollectionChangeType == CollectionChangeType.Add) queue.EnqueueObject(Item);
		else if (CollectionChangeType == CollectionChangeType.Remove) queue.EnqueueObject(Item);
	}
}