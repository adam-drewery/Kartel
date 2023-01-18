using Kartel.EventArgs;

namespace Kartel.Api.Hubs.Base.Interfaces;

public interface ICollectionNotifier
{
	void OnCollectionChanged(object sender, CollectionChangedArgs e);
}