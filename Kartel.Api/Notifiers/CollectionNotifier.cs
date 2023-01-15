using Kartel.Api.Hubs.Base;
using Kartel.Api.Hubs.Base.Interfaces;
using Kartel.EventArgs;
using Microsoft.AspNetCore.SignalR;

namespace Kartel.Api.Notifiers;

/// <summary>Notifies a client when a collection it is subscribed to changes, or one of the entities in the collection.</summary>
public abstract class CollectionNotifier<THub> : EntityNotifier<THub>, ICollectionNotifier where THub : BaseHub
{
	protected CollectionNotifier(IHubContext<THub> hubContext) : base(hubContext) { }

	protected void Watch<TElement>(GameCollection<TElement> collection)
		where TElement : GameObject
	{
		collection.CollectionChanged += OnCollectionChanged;

		foreach (var element in collection) 
			element.PropertyChanged += (_, args) => OnPropertyChanged(args);
	}

	public void OnCollectionChanged(object sender, CollectionChangedArgs e)
	{
		foreach (var gameObject in e.AddedItems) 
			gameObject.PropertyChanged += OnPropertyChanged;
			
		foreach (var gameObject in e.RemovedItems)
			gameObject.PropertyChanged -= OnPropertyChanged;
	}
}