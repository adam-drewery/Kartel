using Kartel.Api.Hubs.Base.Interfaces;

namespace Kartel.Api.Hubs.Base;

public abstract class EntityHub<T> : BaseHub, IEntityHub<T>
{
	protected EntityHub(Game game) : base(game) { }
		
	protected abstract Task<T> LoadData(Guid id);

	public async Task<T> Subscribe(Guid id)
	{
		await Groups.AddToGroupAsync(Context.ConnectionId, id.ToString());
		return await LoadData(id);
	}
		
	public Task Unsubscribe(Guid id)
	{
		return Groups.RemoveFromGroupAsync(Context.ConnectionId, id.ToString());
	}
}