namespace Kartel.Api.Hubs.Base;

public abstract class CollectionHub<T> : BaseHub
{
	protected CollectionHub(Game game) : base(game) { }

	protected abstract Task<List<T>> LoadData(Guid id);

	public async Task<List<T>> Subscribe(Guid id)
	{
		await Groups.AddToGroupAsync(Context.ConnectionId, id.ToString());
		return await LoadData(id);
	}

	public Task Unsubscribe(Guid id)
	{
		return Groups.RemoveFromGroupAsync(Context.ConnectionId, id.ToString());
	}
}