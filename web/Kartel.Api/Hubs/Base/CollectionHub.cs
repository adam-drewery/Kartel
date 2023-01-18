namespace Kartel.Api.Hubs.Base;

public abstract class CollectionHub<T> : BaseHub
{
	protected CollectionHub(Game game) : base(game) { }

	protected abstract Task<IEnumerable<T>> LoadData(Guid id);

	public async Task<IEnumerable<T>> Subscribe(Guid id)
	{
		await Groups.AddToGroupAsync(Context.ConnectionId, id.ToString());
		return await LoadData(id);
	}

	public Task Unsubscribe(Guid id)
	{
		return Groups.RemoveFromGroupAsync(Context.ConnectionId, id.ToString());
	}
}