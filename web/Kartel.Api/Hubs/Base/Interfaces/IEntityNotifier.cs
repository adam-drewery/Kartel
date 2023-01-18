using Kartel.EventArgs;

namespace Kartel.Api.Hubs.Base.Interfaces;

public interface IEntityNotifier : INotifier
{
	void OnPropertyChanged(PropertyChangedArgs args);
}