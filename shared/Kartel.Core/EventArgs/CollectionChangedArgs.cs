using System;
using System.Collections.Generic;
using System.Linq;

namespace Kartel.EventArgs;

public class CollectionChangedArgs
{
	public CollectionChangedArgs(IEnumerable<GameObject> removedItems, IEnumerable<GameObject> addedItems)
	{
		RemovedItems = new List<GameObject>(removedItems);
		RemovedItemIds = new List<Guid>(RemovedItems.Select(x => x.Id));
		AddedItems = new List<GameObject>(addedItems);
	}

	public IReadOnlyCollection<Guid> RemovedItemIds { get; }

	public IReadOnlyCollection<GameObject> RemovedItems { get; }

	public IReadOnlyCollection<GameObject> AddedItems { get; }
}