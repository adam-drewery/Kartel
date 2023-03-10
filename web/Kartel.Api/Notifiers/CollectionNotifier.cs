using Kartel.Api.Hubs.Base;
using Kartel.Api.Hubs.Base.Interfaces;
using Kartel.EventArgs;
using Kartel.Observables;
using Microsoft.AspNetCore.SignalR;

namespace Kartel.Api.Notifiers;

/// <summary>Notifies a client when a collection it is subscribed to changes, or one of the entities in the collection.</summary>
public abstract class CollectionNotifier<THub> : EntityNotifier<THub>, ICollectionNotifier where THub : BaseHub
{
	protected CollectionNotifier(IHubContext<THub> hubContext) : base(hubContext) { }

	protected void Watch<TElement>(ObservableCollection<TElement> collection)
		where TElement : GameObject
	{
		collection.CollectionChanged += OnCollectionChanged;

		foreach (var element in collection) 
			element.PropertyChanged += (_, args) => OnPropertyChanged(args);
	}

	public void OnCollectionChanged(object? sender, CollectionChangedArgs e)
	{
		if (e.Item == null) return;
		
		if (e.CollectionChangeType == CollectionChangeType.Add) 
			e.Item.PropertyChanged += OnPropertyChanged;
			
		else if (e.CollectionChangeType == CollectionChangeType.Remove)
			e.Item.PropertyChanged -= OnPropertyChanged;
	}
}